namespace ShopProject.Controllers
{
    using ForumApp.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Models;

    public class LikePostController : Controller
    {
        private ApplicationDbContext context;

        public LikePostController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("All", "Post");
            }

            string authorId = context.Users
                .FirstOrDefault(t => t.UserName == User.Identity.Name)
                .Id;
            LikePost likePost = new LikePost(id, authorId, null);

            if (context.LikePosts.Any(p => p.PostId == id && p.AuthorId == authorId && p.Dis == null))
            {
                return RedirectToAction("All", "Post");
            }
            else
            {
                context.LikePosts.Add(likePost);
                context.SaveChanges();
                return RedirectToAction("Details", "Post");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateDis(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("All", "Post");
            }

            string authorId = context.Users
                .FirstOrDefault(t => t.UserName == User.Identity.Name)
                .Id;

            LikePost likePost = new LikePost(id, authorId, "Dis");
            if (context.LikePosts.Any(p => p.PostId == id && p.AuthorId == authorId && p.Dis == "Dis"))
            {
                return RedirectToAction("All", "Post");
            }
            else
            {
                context.LikePosts.Add(likePost);
                context.SaveChanges();

                return RedirectToAction("Details", "Post");
            }
        }
    }
}