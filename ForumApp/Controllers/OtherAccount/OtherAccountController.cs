using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        public IActionResult Other(string id)
        {
            ApplicationUser user = context.Users.Include(p => p.UserPosts).Include(c => c.UserComments).FirstOrDefault(u => u.Id == id);
            List<Category> categories = context.UserCategories.Where(c => c.User.Id == id).ToList();

            ViewData["Categories"] = categories;
            return View(user);
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