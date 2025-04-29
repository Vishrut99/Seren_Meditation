using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MEDIT.Models;
using System.Linq;
using System.Threading.Tasks;
using MeditationPlatform.Common;
using Microsoft.EntityFrameworkCore;

namespace MEDIT.Controllers
{
    [Authorize]
    public class ProgressTrackingController : Controller
    {
        private readonly MeditDbContext _context;

        public ProgressTrackingController(MeditDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var sessions = _context.ProgressTrackings
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CompletedOn)
                .ToList();
            var z=sessions.Count();
            var totalMinutes = z*12; // Placeholder if needed later
            var totalSessions = sessions.Sum(p => p.CompletedSessionsCount);
            var lastSessionDate = sessions.FirstOrDefault()?.CompletedOn?.ToString("MMMM dd, yyyy") ?? "N/A";

            var progress = new
            {
                TotalSessions = totalSessions,
                TotalMinutes = totalMinutes,
                LastSessionDate = lastSessionDate,
                DailyProgress = sessions.Select(s => new { Date = s.CompletedOn?.ToString("MMMM dd, yyyy") ?? "N/A", Minutes = 0 }).ToList()
            };

            return View(progress);
        }
        [HttpPost("Progress/ToggleComplete/{id}")]
        public async Task<IActionResult> ToggleComplete(int id, [FromBody] int completedSessionsCount)
        {
            var userId = GetCurrentUserId();
            var tracking = await _context.ProgressTrackings
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (tracking == null)
            {
                tracking = new ProgressTrackings
                {
                    UserId = userId,
                    CompletedSessionsCount = 1, // Start with 1 for the first click
                    CompletedOn = DateTime.Now
                };
                _context.ProgressTrackings.Add(tracking);
            }
            else
            {
                tracking.CompletedSessionsCount += 1; // Increment by 1 each click
                tracking.CompletedOn = DateTime.Now;
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { completedSessionsCount = tracking.CompletedSessionsCount });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        private int GetCurrentUserId()
        {
            if (GlobalUserStore.CurrentUser == null)
            {
                Console.WriteLine("No user logged in.");
                HttpContext.Response.Redirect("/Account/Login");
                //throw new Exception("No user logged in.");
            }
            Console.WriteLine("Current UserId: " + GlobalUserStore.CurrentUser.Id);
            return GlobalUserStore.CurrentUser.Id;
        }


    }
}
