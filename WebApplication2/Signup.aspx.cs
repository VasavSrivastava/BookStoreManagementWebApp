using System;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication2.Data;

namespace WebApplication2
{
    public partial class Signup : System.Web.UI.Page
    {
        private readonly string userFilePath;
        private readonly SessionManager _sessionManager;

        public Signup()
        {
            userFilePath = HttpContext.Current.Server.MapPath("~/App_Data/users.txt");
            _sessionManager = new SessionManager();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (_sessionManager.IsUserLoggedIn())
                {
                    Response.Redirect("~/Dashboard.aspx");
                    return;
                }

                // Ensure users file exists
                if (!File.Exists(userFilePath))
                {
                    string dirPath = Path.GetDirectoryName(userFilePath);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    File.Create(userFilePath).Close();
                }
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string email = txtEmail.Text.Trim();

                // Additional validation
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
                {
                    lblMessage.Text = "All fields are required.";
                    return;
                }

                // Check if username already exists
                if (UserExists(username))
                {
                    lblMessage.Text = "Username already exists. Please choose another.";
                    return;
                }

                // Create user record: username:password:email
                string userRecord = $"{username}:{password}:{email}";

                // Use StreamWriter to append the new user
                using (StreamWriter sw = File.AppendText(userFilePath))
                {
                    sw.WriteLine(userRecord);
                }

                // Log the successful registration
                LogRegistrationAttempt(username, true, "Registration successful");

                // Clear the form
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                txtEmail.Text = string.Empty;

                // Show success message and redirect
                lblMessage.CssClass = "success-message";
                lblMessage.Text = "Account created successfully! Redirecting to login page...";

                // Redirect to login page after 3 seconds
                string script = "setTimeout(function() { window.location = 'Login.aspx'; }, 3000);";
                ClientScript.RegisterStartupScript(this.GetType(), "redirect", script, true);
            }
            catch (Exception ex)
            {
                LogRegistrationAttempt(txtUsername.Text.Trim(), false, $"Error: {ex.Message}");
                lblMessage.Text = "An error occurred during registration. Please try again.";
            }
        }

        private bool UserExists(string username)
        {
            try
            {
                if (File.Exists(userFilePath))
                {
                    string[] users = File.ReadAllLines(userFilePath);
                    return users.Any(line =>
                    {
                        string[] parts = line.Split(':');
                        return parts.Length >= 1 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase);
                    });
                }
                return false;
            }
            catch (Exception ex)
            {
                LogRegistrationAttempt(username, false, $"Error checking user existence: {ex.Message}");
                return false;
            }
        }

        private void LogRegistrationAttempt(string username, bool success, string message)
        {
            try
            {
                string logPath = HttpContext.Current.Server.MapPath("~/App_Data/registration_log.txt");
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