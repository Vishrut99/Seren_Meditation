using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MeditationPlatform.Common;
using MEDIT.Models;

namespace MeditationPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly MeditDbContext _context;

        public HomeController(MeditDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Console.WriteLine("Home/Index action called");
            Console.WriteLine($"GlobalUserStore.IsUserLoggedIn: {GlobalUserStore.IsUserLoggedIn}");
            Console.WriteLine($"GlobalUserStore.CurrentUser: {(GlobalUserStore.CurrentUser != null ? GlobalUserStore.CurrentUser.Name : "null")}");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}