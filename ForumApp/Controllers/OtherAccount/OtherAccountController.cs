using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class OtherAccountController : Controller
    {
        private ApplicationDbContext context;

        public OtherAccountController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int? id)
        {
            return View();
        }

        public IActionResult ListUsers(string searchUser)
        {
            if (searchUser != null)
            {
                List<ApplicationUser> users =
                    context.Users.Where(u => u.FirstName == searchUser && u.UserName != User.Identity.Name || u.LastName == searchUser && u.UserName != User.Identity.Name)
                        .ToList();

                return View(users);
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Other
        (
            string id,
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
                        .Include(c => c.Comments)
                        .Where(u => u.User.Id == id)
                        .Where(p => p.Title.Contains(searchString))
                        .Where(p => p.IsPublic)
                        .OrderByDescending(p => p.CreatedDate)
                        select s;
            }
            else
            {
                posts = from s in context.UserPosts
                        .Include(t => t.User)
                        .Include(c => c.Category)
                        .Include(c => c.Comments)
                        .Where(u => u.User.Id == id)
                        .Where(p => p.IsPublic)
                        .OrderByDescending(p => p.CreatedDate)
                        select s;
            }
            List<SharedPost> sharedPosts = context.SharedPosts.Include(u => u.User).Include(c => c.SharedPostComments).Where(p => p.User.Id == id).OrderByDescending(d => d.SahredDate).ToList();
            ViewData["SharedPosts"] = sharedPosts;

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

        [Authorize]
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "OtherAccount");
            }

            Post post = context.UserPosts.Include(p => p.User).FirstOrDefault(p => p.Id == id);

            return View(post);
        }
    }
}