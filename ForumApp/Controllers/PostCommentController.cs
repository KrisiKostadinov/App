using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class PostCommentController : Controller
    {
        private ApplicationDbContext context;

        public PostCommentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/Index/{id}/PostComment/Create")]
        public IActionResult Create(int? id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("/Post/Index/{PostId}/PostComment/Create")]
        public IActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                Post post = context.UserPosts.Include(t => t.User).FirstOrDefault(p => p.Id == comment.PostId);

                ApplicationUser authorUserFromCreated =
                    context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                string authorUserFromCreatedToStr = authorUserFromCreated.UserName;

                string authorId = post.UserId;

                comment.AuthorId = authorId;

                comment.AuthorIdFromCreated = authorUserFromCreatedToStr;
                context.UserComments.Add(comment);

                context.SaveChanges();
                return Redirect($"/User/Index/PostComment/ShowComments?id={post.Id}");
            }
            return View(comment);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Comment comment = context.UserComments.Find(id);
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            Comment dbComment = context.UserComments.Include(t => t.Author).Include(p => p.Post).FirstOrDefault(c => c.Id == comment.Id);

            if (dbComment.Author.UserName == User.Identity.Name)
            {
                dbComment.Description = comment.Description;
                context.SaveChanges();
            }
            return Redirect($"/User/Index/PostComment/ShowComments?id={dbComment.Post.Id}");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("/User/Index");
            }
            Comment comment = context.UserComments.Include(t => t.Author).Include(p => p.Post).FirstOrDefault(c => c.Id == id);
            if (comment.Author.UserName == User.Identity.Name)
            {
                context.UserComments.Remove(comment);
                context.SaveChanges();
            }
            return Redirect($"/User/Index/PostComment/ShowComments?id={comment.Post.Id}");
        }

        [Route("/User/Index/PostComment/ShowComments")]
        public IActionResult ShowComments(Post post)
        {
            ViewData["Post"] = context.UserPosts.FirstOrDefault(p => p.Id == post.Id);
            return View(context.UserComments.Include(t => t.Author).Include(p => p.Post).Where(p => p.PostId == post.Id).ToList());
        }
    }
}