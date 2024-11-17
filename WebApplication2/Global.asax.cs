using System;
using System.IO;
using System.Web;
using System.Linq;
using WebApplication2.Data;

namespace WebApplication2
{
    public class Global : System.Web.HttpApplication
    {
        private string userFilePath;

        // Remove constructor and initialize userFilePath in Session_Start
        protected void Session_Start(object sender, EventArgs e)
        {
            try
            {
                // Initialize file path here where HttpContext is available
                userFilePath = Server.MapPath("~/App_Data/users.txt");

                var sessionManager = new SessionManager();
                var profileManager = new UserProfileManager();

                if (sessionManager.ValidateAuthCookie())
                {
                    HttpCookie authCookie = Request.Cookies["AuthCookie"];
                    if (authCookie != null)
                    {
                        string userId = authCookie.Values["UserID"];
                        string username = authCookie.Values["Username"];

                        if (ValidateUserFromFile(username))
                        {
                            var userDetails = GetUserDetails(username);
                            if (!string.IsNullOrEmpty(userDetails.Email))
                            {
                                var userProfile = new UserProfile
                                {
                                    UserId = userId,
                                    Username = username,
                                    Email = userDetails.Email,
                                    IsAdmin = username.ToLower() == "admin",
                                    LastLogin = DateTime.Now,
                                    Theme = sessionManager.GetUserPreference("Theme") ?? "light",
                                    Language = sessionManager.GetUserPreference("Language") ?? "en"
                                };

                                profileManager.SaveUserProfile(userProfile);
                                LogAutoLogin(username, true, "Auto-login successful");
                            }
                        }
                        else
                        {
                            sessionManager.RemoveAuthCookie();
                            LogAutoLogin(username, false, "User not found in database");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAutoLogin("Unknown", false, $"Auto-login error: {ex.Message}");
            }
        }

        private bool ValidateUserFromFile(string username)
        {
            try
            {
                if (File.Exists(userFilePath))
                {
                    string[] users = File.ReadAllLines(userFilePath);
                    return users.Any(line =>
                    {
                        string[] parts = line.Split(':');
                        return parts.Length >= 3 && parts[0] == username;
                    });
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private (string Email, string Password) GetUserDetails(string username)
        {
            try
            {
                if (File.Exists(userFilePath))
                {
                    string[] users = File.ReadAllLines(userFilePath);
                    var userLine = users.FirstOrDefault(line =>
                    {
                        string[] parts = line.Split(':');
                        return parts.Length >= 3 && parts[0] == username;
                    });

                    if (userLine != null)
                    {
                        string[] parts = userLine.Split(':');
                        return (Email: parts[2], Password: parts[1]);
                    }
                }
                return (Email: string.Empty, Password: string.Empty);
            }
            catch
            {
                return (Email: string.Empty, Password: string.Empty);
            }
        }

        private void LogAutoLogin(string username, bool success, string message)
        {
            try
            {
                string logPath = Server.MapPath("~/App_Data/autologin_log.txt");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{username}|{success}|{message}|{Request.UserHostAddress}";

                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine(logEntry);
                }
            }
            catch
            {
                // Silently fail if logging fails
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}