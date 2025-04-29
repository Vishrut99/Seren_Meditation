using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace YourProjectName.Controllers
{
    public class MeditationTracksController : Controller
    {
        public IActionResult Index()
        {
            // Static data abhi ke liye
            var tracks = new List<object>
            {
                new { TrackID = 1, Title = "Morning Calm", Duration = 10, Category = "Relaxation", FilePath = "https://example.com/morning-calm.mp4", SubscriptionLevel = "Basic" },
                new { TrackID = 2, Title = "Deep Sleep", Duration = 20, Category = "Sleep", FilePath = "https://example.com/deep-sleep.mp4", SubscriptionLevel = "Pro" },
                new { TrackID = 3, Title = "Focus Flow", Duration = 15, Category = "Focus", FilePath = "https://example.com/focus-flow.mp4", SubscriptionLevel = "Premium" }
            };
            return View(tracks);
        }

        [HttpPost]
        public IActionResult CompleteTrack(int trackId)
        {
            // Simulate track completion
            Console.WriteLine($"Track {trackId} completed on {DateTime.Now}. Adding to progress...");
            return RedirectToAction("Index");
        }
    }
}