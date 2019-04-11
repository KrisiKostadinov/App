using System;
using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers.OtherAccount
{
    public class OtherPostCommentController : Controller
    {
        private ApplicationDbContext context;

        public OtherPostCommentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("/OtherAccount/Other/{id}/OtherPostComment/Create")]
        public IActionResult Create(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("/OtherAccount/Other/{PostId}/OtherPostComment/Create")]
        public IActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                string authorId = context.Users
                    .FirstOrDefault(u => u.UserName == User.Identity.Name)
                    .Id;

                comment.AuthorId = authorId;
                Post post = context.UserPosts.FirstOrDefault(p => p.Id == comment.PostId);

                comment.Post = post;

                comment.AuthorId = authorId;

                context.UserComments.Add(comment);
                context.SaveChanges();

                return Redirect($"/OtherAccount/Other/{authorId}");
            }

            return View(comment);
        }
    }
}