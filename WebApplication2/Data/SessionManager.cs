using System;
using System.Web;

namespace WebApplication2.Data
{
    public class SessionManager
    {
        private const string USER_ID_KEY = "UserID";
        private const string USERNAME_KEY = "Username";
        private const string USER_EMAIL_KEY = "UserEmail";
        private const string IS_ADMIN_KEY = "IsAdmin";
        private const string LAST_LOGIN_KEY = "LastLogin";
        private const string AUTH_COOKIE_NAME = "AuthCookie";
        private const string PREFERENCES_COOKIE_NAME = "UserPreferences";

        private HttpContext Context
        {
            get { return HttpContext.Current ?? throw new InvalidOperationException("HttpContext is not available"); }
        }

        public void SetSessionUser(string userId, string username, string email, bool isAdmin)
        {
            try
            {
                var context = Context;
                context.Session[USER_ID_KEY] = userId;
                context.Session[USERNAME_KEY] = username;
                context.Session[USER_EMAIL_KEY] = email;
                context.Session[IS_ADMIN_KEY] = isAdmin;
                context.Session[LAST_LOGIN_KEY] = DateTime.Now;
            }
            catch (InvalidOperationException)
            {
                // Handle the case where HttpContext is not available
                throw new InvalidOperationException("Cannot set session user: HttpContext is not available");
            }
        }

        public bool IsUserLoggedIn()
        {
            try
            {
                var context = HttpContext.Current;
                return context.Session?["IsAuthenticated"] != null &&
                       (bool)context.Session["IsAuthenticated"];
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SessionManager Exception: {ex.Message}");
                return false;
            }
        }

        public string GetCurrentUsername()
        {
            try
            {
                var context = Context;
                return context.Session?[USERNAME_KEY]?.ToString();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool IsAdmin()
        {
            try
            {
                var context = Context;
                return context.Session?[IS_ADMIN_KEY] != null &&
                       (bool)context.Session[IS_ADMIN_KEY];
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void ClearSession()
        {
            try
            {
                var context = Context;
                context.Session?.Clear();
                context.Session?.Abandon();
            }
            catch (InvalidOperationException)
            {
                // Handle the case where HttpContext is not available
            }
        }

        public void SetAuthCookie(string userId, string username, bool rememberMe)
        {
            try
            {
                var context = Context;
                HttpCookie authCookie = new HttpCookie(AUTH_COOKIE_NAME);
                authCookie.Values["UserID"] = userId;
                authCookie.Values["Username"] = username;

                if (rememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(30);
                }

                authCookie.HttpOnly = true;
                context.Response.Cookies.Add(authCookie);
            }
            catch (InvalidOperationException)
            {
                // Handle the case where HttpContext is not available
            }
        }

        public void RemoveAuthCookie()
        {
            try
            {
                var context = Context;
                if (context.Request.Cookies[AUTH_COOKIE_NAME] != null)
                {
                    HttpCookie authCookie = context.Request.Cookies[AUTH_COOKIE_NAME];
                    authCookie.Expires = DateTime.Now.AddDays(-1);
                    context.Response.Cookies.Add(authCookie);
                }
            }
            catch (InvalidOperationException)
            {
                // Handle the case where HttpContext is not available
            }
        }

        public bool ValidateAuthCookie()
        {
            try
            {
                var context = Context;
                HttpCookie authCookie = context.Request.Cookies[AUTH_COOKIE_NAME];
                return authCookie != null &&
                       !string.IsNullOrEmpty(authCookie.Values["UserID"]);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void SetUserPreference(string key, string value)
        {
            try
            {
                var context = Context;
                HttpCookie prefCookie = context.Request.Cookies[PREFERENCES_COOKIE_NAME]
                                      ?? new HttpCookie(PREFERENCES_COOKIE_NAME);

                prefCookie.Values[key] = value;
                prefCookie.Expires = DateTime.Now.AddYears(1);

                context.Response.Cookies.Add(prefCookie);
            }
            catch (InvalidOperationException)
            {
                // Handle the case where HttpContext is not available
            }
        }

        public string GetUserPreference(string key)
        {
            try
            {
                var context = Context;
                HttpCookie prefCookie = context.Request.Cookies[PREFERENCES_COOKIE_NAME];
                return prefCookie?.Values[key];
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}