using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MEDIT.Models;
using MeditationPlatform.Common;

namespace MeditationPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly MeditDbContext _context;

        public AccountController(MeditDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.Password != password)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                Console.WriteLine("Login failed: Invalid credentials");
                return View();
            }

            GlobalUserStore.CurrentUser = user;
            Console.WriteLine($"Login successful: GlobalUserStore.CurrentUser set to {GlobalUserStore.CurrentUser?.Name ?? "null"}");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "The password and confirmation password do not match.");
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Data saved successfully.");

                    GlobalUserStore.CurrentUser = user;
                    Console.WriteLine($"Register successful: GlobalUserStore.CurrentUser set to {GlobalUserStore.CurrentUser?.Name ?? "null"}");

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    ModelState.AddModelError("", "An error occurred while saving the user. Please try again.");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            Console.WriteLine("Logout called, clearing GlobalUserStore");
            GlobalUserStore.ClearUser();
            return RedirectToAction("Login", "Account");
        }
    }
}