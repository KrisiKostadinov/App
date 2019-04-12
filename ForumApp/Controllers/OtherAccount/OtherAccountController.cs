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
                    context.Users.Where(u => u.FirstName == searchUser || u.LastName == searchUser)
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
                    .Where(u => u.User.Id == id)
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

            ViewData["Categories"] = context.UserCategories.Include(u => u.User).Include(p => p.UserPosts).Where(c => c.User.Id == id).ToList();

            ViewData["User"] = context.Users.FirstOrDefault(u => u.Id == id);
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