using System;
using System.Collections.Generic;
using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopProject.Models;

namespace ShopProject.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext context;

        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet]
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
                if (category.Title == "")
                {
                    ViewData["Error"] = "Empty field!";
                    return View();
                }

                List<string> categoryTitles = context.Categories
                    .Select(c => c.Title)
                    .ToList();
                if (!categoryTitles.Contains(category.Title))
                {
                    string authorId = context.Users
                        .FirstOrDefault(t => t.UserName == User.Identity.Name)
                        .Id;

                    category.AuthorId = authorId;
                    ApplicationUser user = context.Users
                        .FirstOrDefault(t => t.UserName == User.Identity.Name);
                    category.Author = user;

                    category.CreatedDate = DateTime.Now;
                    context.Categories.Add(category);

                    context.SaveChanges();

                    ViewData["DelAll"] = category.Title;

                    return RedirectToAction("All", "Post");
                }
                else
                {
                    ViewData["Error"] = $"{category.Title} exist!";
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            Category category = context.Categories
                .Include(c => c.Author)
                .FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Title == null)
                {
                    ViewData["Error"] = "Empty field!";
                    return View();
                }

                Category dbCategory = context.Categories
                    .Include(t => t.Author)
                    .FirstOrDefault(c => c.Id == category.Id);

                dbCategory.Title = category.Title;

                dbCategory.CreatedDate = DateTime.Now;
                context.SaveChanges();
            }
            return RedirectToAction("All", "Post");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category dbCategory = context.Categories
                .Include(t => t.Author)
                .FirstOrDefault(c => c.Id == id &&
                                     c.Author.UserName == User.Identity.Name);

            if (dbCategory == null)
            {
                return RedirectToAction("All", "Post");
            }

            List<Post> posts = context.Posts
                .Where(p => p.Category.Id == id)
                .ToList();

            context.RemoveRange(posts);
            context.Categories.Remove(dbCategory);
            context.SaveChanges();
            return RedirectToAction("All", "Post");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteAll()
        {
            List<Post> posts = context.Posts.Where(p => p.Author.UserName == User.Identity.Name).ToList();
            List<Category> categories = context.Categories.Where(c => c.Author.UserName == User.Identity.Name).ToList();

            if (categories.Any() && !posts.Any())
            {
                ViewData["Error"] = "You don't this category becouse, she contains posts of other users!";

                return RedirectToAction("All", "Post");
            }
            context.Posts.RemoveRange(posts);

            context.Categories.RemoveRange(categories);

            context.SaveChanges();

            return RedirectToAction("All", "Post");
        }
    }
}