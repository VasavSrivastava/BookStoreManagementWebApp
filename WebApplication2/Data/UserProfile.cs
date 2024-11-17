using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Data
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLogin { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
    }

    public class UserProfileManager
    {
        private readonly SessionManager _sessionManager;

        public UserProfileManager()
        {
            _sessionManager = new SessionManager();
        }

        public void SaveUserProfile(UserProfile profile)
        {
            // Save to session
            _sessionManager.SetSessionUser(
                profile.UserId,
                profile.Username,
                profile.Email,
                profile.IsAdmin
            );

            // Save preferences to cookies
            _sessionManager.SetUserPreference("Theme", profile.Theme);
            _sessionManager.SetUserPreference("Language", profile.Language);
        }

        public UserProfile GetCurrentUserProfile()
        {
            if (!_sessionManager.IsUserLoggedIn())
                return null;

            return new UserProfile
            {
                Username = _sessionManager.GetCurrentUsername(),
                Theme = _sessionManager.GetUserPreference("Theme") ?? "light",
                Language = _sessionManager.GetUserPreference("Language") ?? "en",
                IsAdmin = _sessionManager.IsAdmin()
            };
        }
    }
}