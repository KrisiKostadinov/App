using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumApp.Data;
using ForumApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ShopProject.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext context;

        public PostController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("/Post/Create")]
        public IActionResult Create()
        {
            List<string> categories = context.Categories.Select(c => c.Title).ToList();
            ViewData["CategoryNames"] = categories;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Post post, string categoryTitle)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;

                post.LastCreatedDate = DateTime.Now;

                string authorId = context.Users
                    .Where(t => t.UserName == User.Identity.Name)
                    .First()
                    .Id;

                post.AuthorId = authorId;

                ApplicationUser user = context.Users
                    .FirstOrDefault(t => t.UserName == User.Identity.Name);

                post.Author = user;
                context.Posts.Add(post);

                Category dbCategory = context.Categories.FirstOrDefault(c => c.Title == categoryTitle);
                post.Category = dbCategory;
                ViewData["Sucess"] = "Sucess created new post!";
                ViewData["CreatedCategory"] = post.Category.Title;
                context.SaveChanges();

                return View();
            }
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> All
        (
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var posts = from s in context.Posts
                    .Include(t => t.Author)
                    .Include(c => c.Category)
                    .ThenInclude(t => t.Author)
                select s;

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
            List<Category> categories = context.Categories
                .ToList();
            ViewData["Categories"] = categories;

            int pageSize = 3;

            List<int> postsLater = context.PostLaters
                .Include(t => t.Author)
                .Where(p => p.Author.UserName == User.Identity.Name)
                .Select(p => p.PostId)
                .ToList();

            ViewData["postsLater"] = postsLater;

            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize]
        [HttpGet]
        public IActionResult SelectPosts(int? id)
        {
            List<Post> posts = context.Posts
                .Include(t => t.Author)
                .Include(c => c.Category)
                .Where(c => c.Category.Id == id)
                .OrderByDescending(t => t.CreatedDate)
                .ThenByDescending(t => t.LastCreatedDate)
                .ToList();
            Post dbPost = context.Posts.Include(c => c.Category).FirstOrDefault(p => p.Id == id);
            Category category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (id != null)
            {
                if (posts.Any())
                {
                    ViewData["Category"] = $"{category.Title} {posts.Count}";
                    ViewData["DelAll"] = $"{category.Title}";
                }
                else
                {
                    if (category != null)
                    {
                        ViewData["Category"] = $"{category.Title} is empty";
                        ViewData["DelAll"] = $"{category.Title}";
                    }
                }
            }
            ViewData["Posts"] = posts;
            List<Category> categories = context.Categories.ToList();

            ViewData["Categories"] = categories;
            return View(category);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("All", "Post");
            }
            Post dbPost = context.Posts
                .Include(t => t.Author)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefault(p => p.Id == id);

            LikesCalculated(dbPost);
            if (dbPost == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(dbPost);
        }

        private void LikesCalculated(Post dbPost)
        {
            List<int> ids = context.LikePosts.Where(l => l.Dis == null).Select(p => p.PostId).ToList();
            List<int> idsDis = context.LikePosts.Where(l => l.Dis == "Dis").Select(l => l.PostId).ToList();

            if (ids.Contains(dbPost.Id) || idsDis.Contains(dbPost.Id))
            {
                int likes = ids
                    .Where(l => l == dbPost.Id)
                    .ToList()
                    .Count;

                int dislikes = idsDis
                    .Where(l => l == dbPost.Id)
                    .ToList()
                    .Count;

                dbPost.LikeCount = likes;
                dbPost.DislikeCount = dislikes;
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Post dbPost = context.Posts
                .Include(t => t.Author)
                .FirstOrDefault(p => p.Id == id);

            if (dbPost == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (dbPost.Author.UserName == User.Identity.Name)
            {
                List<string> categoryNames = context.Categories.Select(c => c.Title).ToList();

                ViewData["CategoryNames"] = categoryNames;
                return View(dbPost);
            }

            return RedirectToAction("All", "Post");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                Post dbPost = context.Posts
                    .Include(t => t.Author)
                    .Include(t => t.Category)
                    .FirstOrDefault(p => p.Id == post.Id);
                if (dbPost.Author.UserName == User.Identity.Name)
                {
                    dbPost.Description = post.Description;
                    dbPost.Title = post.Title;
                    dbPost.Subtitle = post.Subtitle;

                    dbPost.LastCreatedDate = DateTime.Now;

                    context.SaveChanges();

                    return RedirectToAction("All");
                }
                else
                {
                    return RedirectToAction("All", "Post");
                }
            }

            return View(post);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Post dbPost = context.Posts
                .Include(t => t.Author)
                //.Include()
                .FirstOrDefault(p => p.Id == id);
            if (dbPost == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (dbPost.Author.UserName == User.Identity.Name)
            {
                return View(dbPost);
            }
            else
            {
                return RedirectToAction("All", "Post");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(Post post)
        {
            context.Posts.Remove(post);
            context.SaveChanges();

            return RedirectToAction("All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DelAll(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("All");
            }

            Category category = context.Categories
                .Include(t => t.Author)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return RedirectToAction("All");
            }

            List<Post> delPosts = context.Posts
                .Where(p => p.Category.Id == id &&
                            p.Author.UserName == User.Identity.Name)
                .ToList();
            context.Posts.RemoveRange(delPosts);
            context.SaveChanges();

            return RedirectToAction("All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAll()
        {
            List<Post> posts = context.Posts.Where(p => p.Author.UserName == User.Identity.Name).ToList();

            context.Posts.RemoveRange(posts);
            context.SaveChanges();

            return RedirectToAction("All", "Post");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AllPostsForLater()
        {
            List<Post> posts = context.PostLaters
                .Include(t => t.Author)
                .Where(p => p.Author.UserName == User.Identity.Name)
                .Select(p => p.Post)
                .ToList();
            List<Post> postsIncluded = new List<Post>();
            foreach (var post in posts)
            {
                postsIncluded.Add(context.Posts.Include(t => t.Author).Include(c => c.Category).Include(c => c.Comments).FirstOrDefault(p => p.Id == post.Id));
                LikesCalculated(post);
            }

            return View(postsIncluded);
        }
    }
}