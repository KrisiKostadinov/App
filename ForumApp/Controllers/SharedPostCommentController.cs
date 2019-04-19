using System.Collections.Generic;
using System.Linq;
using ForumApp.Data;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class SharedPostCommentController : Controller
    {
        private ApplicationDbContext context;

        public SharedPostCommentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/Index/{id}/SharedPostComment/Create")]
        public IActionResult Create(int? id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("/Post/Index/{SharedPostId}/SharedPostComment/Create")]
        public IActionResult Create(SharedPostComment comment)
        {
            if (ModelState.IsValid)
            {
                string userId = context.Users
                    .FirstOrDefault(u => u.UserName == User.Identity.Name)
                    .Id;
                comment.UserId = userId;

                context.SharedPostComments.Add(comment);
                context.SaveChanges();

                return RedirectToAction("ShowComments", new {id = comment.SharedPostId});
            }
            return View(comment);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShowComments(int id)
        {
            List<SharedPostComment> sharedPostComments = context.SharedPostComments
                .Include(u => u.User)
                .Include(p => p.SharedPost)
                .Where(c => c.SharedPost.Id == id)
                .ToList();
            int sharedPost = context.SharedPosts.FirstOrDefault(p => p.Id == id).Id;
            ViewData["SharedPost"] = sharedPost;

            return View(sharedPostComments);
        }
    }
}