using System;
using ForumApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class SharedPostController : Controller
    {
        private ApplicationDbContext context;

        public SharedPostController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int? id, string success)
        {
            string userId = context.Users
                .FirstOrDefault(u => u.UserName == User.Identity.Name)
                .Id;

            Post post = context.UserPosts.FirstOrDefault(p => p.Id == id);
            int categoryId = context.UserPosts.FirstOrDefault(c => c.Id == post.Id).CategoryId;

            SharedPost sharedUser = new SharedPost();

            sharedUser.UserId = userId;

            sharedUser.UserPostId = id;
            sharedUser.Title = post.Title;

            sharedUser.Description = post.Description;

            sharedUser.CategoryId = categoryId;
            sharedUser.SahredDate = DateTime.Now;

            context.SharedPosts.Add(sharedUser);
            context.SaveChanges();

            return RedirectToAction("Details", "User", new { id, success });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int id)
        {
            SharedPost sharedPost = context.SharedPosts
                .Include(u => u.User)
                .Include(c => c.Category)
                .Include(c => c.SharedPostComments)
                .FirstOrDefault(p => p.Id == id);

            foreach (var sharedPostSharedPostComment in sharedPost.SharedPostComments)
            {
                ApplicationUser user = context.Users.FirstOrDefault(p => p.Id == sharedPostSharedPostComment.UserId);
                sharedPostSharedPostComment.User = user;
            }
            return View(sharedPost);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            SharedPost sharedPost = context.SharedPosts.FirstOrDefault(p => p.Id == id);

            return View(sharedPost);
        }

        public IActionResult Edit(SharedPost sharedPost)
        {
            if (ModelState.IsValid)
            {
                SharedPost dbSharedPost = context.SharedPosts.FirstOrDefault(p => p.Id == sharedPost.Id);
                dbSharedPost.LastUpdatedDate = sharedPost.LastUpdatedDate;

                dbSharedPost.Description = sharedPost.Description;

                dbSharedPost.Title = sharedPost.Title;
                context.SaveChanges();

                return RedirectToAction("Details", new {sharedPost.Id});
            }

            return View(sharedPost);
        }

    }
}