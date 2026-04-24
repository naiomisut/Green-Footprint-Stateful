using System;
using System.Web.Security;
using System.Xml;
namespace YourNamespace
{ 
    public partial class Member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblWelcome.Text = "Welcome, " + User.Identity.Name + "!";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("Default.aspx");
        }
    }
}