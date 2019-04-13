using System.Collections.Generic;
using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ForumApp.Controllers
{
    public class PostLaterController : Controller
    {
        private ApplicationDbContext context;

        public PostLaterController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("All", "Post");
            }

            Post post = context.Posts
                .Include(t => t.Author)
                .Include(c => c.Category)
                .Include(c => c.Comments)
                .FirstOrDefault(p => p.Id == id);

            PostLater postLater = new PostLater();

            ApplicationUser user = context.Users
                .FirstOrDefault(u => u.UserName == User.Identity.Name);

            postLater.Author = user;

            postLater.Post = post;
            post.PostLaters.Add(postLater);

            context.SaveChanges();

            return RedirectToAction("All", "Post");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AllPostsForLater", "Post");
            }

            PostLater postLater = context.PostLaters
                .FirstOrDefault(p => p.PostId == id);

            context.Remove(postLater);

            context.SaveChanges();
            return RedirectToAction("AllPostsForLater", "Post");
        }

        public IActionResult Cancelate(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("All", "Post");
            }

            PostLater postLater = context.PostLaters
                .FirstOrDefault(p => p.PostId == id);
            context.Remove(postLater);
            context.SaveChanges();

            return RedirectToAction("All", "Post");
        }
    }
}