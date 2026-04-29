using system;

namespace YourNamespace
{
    public partial class Staff : System.Web.UI.Page
    {
        // Checking if user is logged in ANDD has staff role
        protected void Page_Load(object sender, EventArgs e)
        {
            // restricting the access
            if (Session["role"] == null || Session["role"].ToString() != "staff")
            {
                // If not staff redirect back to default page
                Response.Redirect("Default.aspx");
            }

            // Show a greeting message
            lblWelcome.Text = "Welcome, Staff!";
        }
    
    protected void btnLogout_Click(Object sender, EventArgs e)
        {
            // Clears the session so user is logged out
            Session.Clear();

            // Sends user to the home page
            Response.Redirect("Default.aspx");
        }
    }
}