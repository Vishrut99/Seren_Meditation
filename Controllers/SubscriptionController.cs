using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MEDIT.Models;
using MeditationPlatform.Common;
using Microsoft.EntityFrameworkCore;

namespace MEDIT.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly MeditDbContext _context;

        public SubscriptionController(MeditDbContext context)
        {
            _context = context;
        }

        // GET: Subscription
        public IActionResult Index()
        {
            Console.WriteLine("Subscription/Index action called");

            // Check if the user has an active subscription
            string purchasedPlan = null;
            if (GlobalUserStore.IsUserLoggedIn)
            {
                var user = _context.Users
                    .Include(u => u.Subscriptions)
                    .FirstOrDefault(u => u.Id == GlobalUserStore.CurrentUser.Id);

                if (user != null && user.Subscriptions != null)
                {
                    var activeSubscription = user.Subscriptions
                        .Where(s => s.EndDate == null || s.EndDate > DateTime.Now)
                        .OrderByDescending(s => s.StartDate)
                        .FirstOrDefault();

                    if (activeSubscription != null)
                    {
                        purchasedPlan = activeSubscription.PlanName;
                        Console.WriteLine($"User has active subscription: {purchasedPlan}");
                    }
                }
            }

            ViewBag.PurchasedPlan = purchasedPlan;
            return View();
        }
        public IActionResult Payment()
        {
            return View(); // This will load Views/Subscription/Payment.cshtml
        }
        public IActionResult Basic()
        {
            if (!GlobalUserStore.IsUserLoggedIn)
            {
                TempData["Message"] = "Please log in to access plans.";
                Console.WriteLine("Redirecting to Login: User not logged in");
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefault(u => u.Id == GlobalUserStore.CurrentUser.Id);

            if (user == null || user.Subscriptions == null || !user.Subscriptions.Any(s => (s.EndDate == null || s.EndDate > DateTime.Now) && s.PlanName == "Basic"))
            {
                TempData["Message"] = "You need to purchase the Basic plan first!";
                Console.WriteLine("Redirecting to Index: No active Basic subscription");
                return RedirectToAction("Index");
            }

            Console.WriteLine("Access granted to Basic plan page");
            return View();
        }

        public IActionResult Pro()
        {
            if (!GlobalUserStore.IsUserLoggedIn)
            {
                TempData["Message"] = "Please log in to access plans.";
                Console.WriteLine("Redirecting to Login: User not logged in");
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefault(u => u.Id == GlobalUserStore.CurrentUser.Id);

            if (user == null || user.Subscriptions == null || !user.Subscriptions.Any(s => (s.EndDate == null || s.EndDate > DateTime.Now) && s.PlanName == "Pro"))
            {
                TempData["Message"] = "You need to purchase the Pro plan first!";
                Console.WriteLine("Redirecting to Index: No active Pro subscription");
                return RedirectToAction("Index");
            }

            Console.WriteLine("Access granted to Pro plan page");
            return View();
        }

        public IActionResult Premium()
        {
            if (!GlobalUserStore.IsUserLoggedIn)
            {
                TempData["Message"] = "Please log in to access plans.";
                Console.WriteLine("Redirecting to Login: User not logged in");
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefault(u => u.Id == GlobalUserStore.CurrentUser.Id);

            if (user == null || user.Subscriptions == null || !user.Subscriptions.Any(s => (s.EndDate == null || s.EndDate > DateTime.Now) && s.PlanName == "Premium"))
            {
                TempData["Message"] = "You need to purchase the Premium plan first!";
                Console.WriteLine("Redirecting to Index: No active Premium subscription");
                return RedirectToAction("Index");
            }

            Console.WriteLine("Access granted to Premium plan page");
            return View();
        }

        // POST: Subscription/Create
        [HttpPost]
        public async Task<IActionResult> Create(string PlanName)
        {
            if (!GlobalUserStore.IsUserLoggedIn)
            {
                TempData["Message"] = "Please log in to purchase a plan.";
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefault(u => u.Id == GlobalUserStore.CurrentUser.Id);

            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            // End any existing active subscription
            var activeSubscription = user.Subscriptions
                .FirstOrDefault(s => s.EndDate == null || s.EndDate > DateTime.Now);

            if (activeSubscription != null)
            {
                activeSubscription.EndDate = DateTime.Now;
                _context.Subscriptions.Update(activeSubscription);
            }

            // Add new subscription
            var newSubscription = new Subscription
            {
                UserId = user.Id,
                PlanName = PlanName,
                StartDate = DateTime.Now,
                EndDate = null
            };

            _context.Subscriptions.Add(newSubscription);
            await _context.SaveChangesAsync();

            return Content($@"
        <script>
          
            window.location.href = '{Url.Action("Payment", "Subscription")}';
        </script>
    ", "text/html");
        }
    }
}