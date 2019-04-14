using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index
        (
            int? id,
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            IQueryable<Post> posts = null;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (searchString != currentFilter)
            {
                posts = from s in context.UserPosts
                        .Include(t => t.User)
                        .Include(c => c.Category)
                        .ThenInclude(t => t.User)
                        .Where(u => u.User.UserName == User.Identity.Name)
                        .Where(p => p.Title.Contains(searchString))
                        .OrderByDescending(p => p.CreatedDate)
                        select s;
            }
            else
            {
                posts = from s in context.UserPosts
                        .Include(t => t.User)
                        .Include(c => c.Comments)
                        .Include(c => c.Category)
                        .ThenInclude(t => t.User)
                        .Where(u => u.User.UserName == User.Identity.Name)
                        .OrderByDescending(p => p.CreatedDate)
                        select s;
            }

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    posts = posts.OrderByDescending(s => s.Title);
                    break;
            }

            int pageSize = 3;

            ViewData["Categories"] = context.UserCategories.Include(p => p.UserPosts).Where(c => c.User.UserName == User.Identity.Name).ToList();

            ViewData["User"] = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        public IActionResult SearchPosts(string search)
        {
            if (search != "")
            {
                List<Post> posts = context.UserPosts.Where(p => p.Title.Contains(search)).ToList();
            }

            return null;
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
        public IActionResult EditUserPost(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "User");
            }

            Post post = context.UserPosts.FirstOrDefault(p => p.Id == id);

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditUserPost(Post post)
        {
            if (ModelState.IsValid)
            {
                Post dbPost = context.UserPosts.FirstOrDefault(p => p.Id == post.Id);
                dbPost.Title = post.Title;

                dbPost.Description = post.Description;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteUserPost(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "User");
            }
            Post post = context.UserPosts.FirstOrDefault(p => p.Id == id);

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteUserPost(Post post)
        {
            context.UserPosts.Remove(post);
            context.SaveChanges();

            return RedirectToAction("Index");
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

            if (category != null)
            {
                if (category.User.UserName == User.Identity.Name)
                {
                    return View(category.UserPosts);
                }
                return View(category.UserPosts.Where(p => p.IsPublic).ToList());
            }

            return View();
        }

        [HttpGet]
        public IActionResult Settings(int? id)
        {
            Post post = context.UserPosts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Settings(Post post)
        {
            Post dbPost = context.UserPosts.FirstOrDefault(p => p.Id == post.Id);
            dbPost.IsPublic = post.IsPublic;
            context.SaveChanges();
            return View(dbPost);
        }
    }
}