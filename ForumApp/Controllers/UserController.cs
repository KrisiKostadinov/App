using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                List<Comment> showComments = context.UserComments.Where(p => p.PostId == id).ToList();
                return PartialView("/Views/PostComment/ShowComments.cshtml", showComments);
            }
            ApplicationUser user = context.Users.Include(p => p.UserPosts).Include(c => c.UserComments).FirstOrDefault(u => u.UserName == User.Identity.Name);
            ViewData["User"] = user;

            List<Category> categories = context.UserCategories.Include(p => p.UserPosts).Where(c => c.User.UserName == User.Identity.Name).ToList();
            ViewData["Categories"] = categories;

            return View(user);
        }

        public IActionResult GetInformation(string country)
        {
            ApplicationUser user = context.
                Users.
                FirstOrDefault(u => u.UserName == User.Identity.Name);
            user.Country = country;

            context.SaveChanges();

            Thread.Sleep(1000);

            return RedirectToAction("Index", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateUserPost(int? id)
        {
            List<Category> categories = context.UserCategories.Where(t => t.User.UserName == User.Identity.Name).ToList();
            ViewData["Categories"] = categories;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateUserPost(Post post, int categoryId)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;
                post.LastUpdatedDate = DateTime.Now;
                ApplicationUser user = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                post.User = user;
                int dbCategoryId = context.UserCategories
                    .FirstOrDefault(c => c.Id == categoryId)
                    .Id;

                post.CategoryId = dbCategoryId;

                context.UserPosts.Add(post);

                context.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Select(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Category category = context.UserCategories.Include(p => p.UserPosts).Include(u => u.User).FirstOrDefault(c => c.Id == id);

            return View(category.UserPosts);
        }
    }
}