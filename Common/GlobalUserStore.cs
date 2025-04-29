using MEDIT.Models;

namespace MeditationPlatform.Common
{
    public static class GlobalUserStore
    {
        public static User? CurrentUser { get; set; } = null;

        public static bool IsUserLoggedIn => CurrentUser != null;

        public static void ClearUser()
        {
            CurrentUser = null;
        }
    }
}