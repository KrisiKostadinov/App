using System;
using System.Collections.Generic;
using System.Linq;
using ForumApp.Data;
using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ShopProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    public class CommentController : Controller
    {
        private ApplicationDbContext context;

        public CommentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/Details/{id}/Comment/Create")]
        public IActionResult Create(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("/Post/Details/{PostId}/Comment/Create")]
        public IActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.Now;

                comment.LastUpdatedDate = DateTime.Now;
                string authorId = context.Users
                    .Where(u => u.UserName == User.Identity.Name)
                    .FirstOrDefault()
                    .Id;

                comment.AuthorId = authorId;

                Post post = context.Posts.Find(comment.PostId);

                post.LastCreatedDate = DateTime.Now;
                context.Comments.Add(comment);

                context.SaveChanges();

                return Redirect($"/Post/Details/{comment.PostId}");
            }
            return RedirectToAction("All", "Post");
        }

        [Authorize]
        [HttpGet]
        public IActionResult _ListComments()
        {
            List<Comment> comments = context.Comments
                .ToList();
            return RedirectToAction("Details", "Post");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AllForPost(int? id)
        {
            if (id == null)
            {
                return Redirect("/Post/AllPostsForLater");
            }

            List<Comment> comments = context.Posts.Include(c => c.Comments).Include(t => t.Author).Include(c => c.Category).FirstOrDefault(p => p.Id == id).Comments;

            return View(comments);
        }
    }
}