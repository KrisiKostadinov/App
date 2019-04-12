using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}