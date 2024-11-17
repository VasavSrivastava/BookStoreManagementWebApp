using System;
using System.IO;
using System.Web;
using System.Linq;
using WebApplication2.Data;

namespace WebApplication2
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly SessionManager _sessionManager;
        private readonly UserProfileManager _profileManager;
        private readonly string userFilePath;

        public Login()
        {
            _sessionManager = new SessionManager();
            _profileManager = new UserProfileManager();
            userFilePath = HttpContext.Current.Server.MapPath("~/App_Data/users.txt");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // If user is already logged in, redirect to welcome page
            if (Session["Username"] != null)
            {
                Response.Redirect("~/WelcomePage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Check for remember me cookie
                HttpCookie authCookie = Request.Cookies["AuthCookie"];
                if (authCookie != null)
                {
                    txtUsername.Text = authCookie["Username"];
                    chkRememberMe.Checked = true;
                }

                // Ensure users file exists
                if (!File.Exists(userFilePath))
                {
                    string dirPath = Path.GetDirectoryName(userFilePath);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    using (StreamWriter sw = File.CreateText(userFilePath))
                    {
                        sw.WriteLine("admin:admin123:admin@example.com");
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (ValidateUser(username, password))
                {
                    // Set session variables
                    Session["Username"] = username;
                    Session["IsAuthenticated"] = true;
                    System.Diagnostics.Debug.WriteLine("Login.aspx.cs: Username = " + Session["Username"]);
                    System.Diagnostics.Debug.WriteLine("Login.aspx.cs: IsAuthenticated = " + Session["IsAuthenticated"]);

                    // Redirect to Welcome Page
                    Response.Redirect("~/WelcomePage.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    lblMessage.Text = "Please enter both username and password.";
                    return;
                }

                string[] users = File.ReadAllLines(userFilePath);
                var userLine = users.FirstOrDefault(line =>
                {
                    string[] parts = line.Split(':');
                    return parts.Length >= 3 &&
                           parts[0] == username &&
                           parts[1] == password;
                });

                if (userLine != null)
                {
                    string[] userData = userLine.Split(':');
                    string email = userData[2];
                    bool isAdmin = username.ToLower() == "admin";

                    // Set session variables
                    Session["Username"] = username;
                    Session["UserEmail"] = email;
                    Session["IsAdmin"] = isAdmin;
                    Session["LastLogin"] = DateTime.Now;

                    // Handle Remember Me
                    if (chkRememberMe.Checked)
                    {
                        HttpCookie authCookie = new HttpCookie("AuthCookie")
                        {
                            Expires = DateTime.Now.AddDays(30)
                        };
                        authCookie.Values["Username"] = username;
                        Response.Cookies.Add(authCookie);
                    }

                    // Log successful login
                    LogLoginAttempt(username, true, "Login successful");

                    // Redirect to welcome page
                    Response.Redirect("~/WelcomePage.aspx");
                }
                else
                {
                    LogLoginAttempt(username, false, "Invalid credentials");
                    lblMessage.Text = "Invalid username or password.";
                    txtPassword.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LogLoginAttempt(txtUsername.Text.Trim(), false, $"Error: {ex.Message}");
                lblMessage.Text = "An error occurred during login. Please try again.";
            }
        }

        private bool ValidateUser(string username, string password)
        {
            // Validate user from database or file
            return username == "admin" && password == "admin"; 
        }
        private void LogLoginAttempt(string username, bool success, string message)
        {
            try
            {
                string logPath = Server.MapPath("~/App_Data/login_log.txt");
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
    }
}