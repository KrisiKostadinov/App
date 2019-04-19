using System.Collections.Generic;
using System.Linq;
using ForumApp.Data;
using ForumApp.Models;
using ForumApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
    public class SharedUserController : Controller
    {
        private ApplicationDbContext context;

        public SharedUserController(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}