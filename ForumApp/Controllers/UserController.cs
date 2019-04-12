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
using System.Web;
using PagedList;

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

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var posts = from s in context.UserPosts
                    .Include(t => t.User)
                    .Include(c => c.Category)
                    .ThenInclude(t => t.User)
                    .Where(u => u.User.UserName == User.Identity.Name)
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

            int pageSize = 3;

            ViewData["Categories"] = context.UserCategories.Where(c => c.User.UserName == User.Identity.Name).ToList();

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