using System;
using WebApplication2.Data;

namespace WebApplication2
{
    public partial class WelcomePage : System.Web.UI.Page
    {
        private readonly SessionManager _sessionManager;

        public WelcomePage()
        {
            _sessionManager = new SessionManager();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
                {
                    //Response.Redirect("~/Login.aspx");
                    System.Diagnostics.Debug.WriteLine("WelcomePage.aspx.cs: Username = " + Session["Username"]);
                    System.Diagnostics.Debug.WriteLine("WelcomePage.aspx.cs: IsAuthenticated = " + Session["IsAuthenticated"]);
                    return;
                }

                // Display the logged-in user's name
                lblUsername.Text = Session["Username"]?.ToString() ?? "Guest";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session and remove cookies
            _sessionManager.ClearSession();
            _sessionManager.RemoveAuthCookie();

            // Redirect to login page
            Response.Redirect("~/Login.aspx");
        }
    }
}
