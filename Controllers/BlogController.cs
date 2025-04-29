using Microsoft.AspNetCore.Mvc;

namespace MEDIT.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
