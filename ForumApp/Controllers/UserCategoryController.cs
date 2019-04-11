using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
    public class UserCategoryController : Controller
    {
        private ApplicationDbContext context;

        public UserCategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                string userId = context.Users
                    .FirstOrDefault(t => t.UserName == User.Identity.Name)
                    .Id;

                category.UserId = userId;
                ApplicationUser user = context.Users
                    .FirstOrDefault(t => t.UserName == User.Identity.Name);

                category.User = user;
                context.UserCategories.Add(category);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "User");
            }

            Category category = context.UserCategories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                Category dbCategory = context.UserCategories.Include(u => u.User).FirstOrDefault(c => c.Id == category.Id);
                if (dbCategory.User.UserName == User.Identity.Name)
                {
                    dbCategory.Title = category.Title;
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "User");
            }

            Category category = context.UserCategories.FirstOrDefault(c => c.Id == id);

            return View(category);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            Category dbCategory = context.UserCategories.Include(c => c.User).FirstOrDefault(c => c.Id == category.Id);
            if (dbCategory.User.UserName == User.Identity.Name)
            {
                context.UserCategories.Remove(dbCategory);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }
    }
}