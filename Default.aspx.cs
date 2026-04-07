using System;
using System.Web.UI;

namespace YourNamespace
{
    public partial class ServicesTryIt : Page
    {
        GreenFootprintService gfService = new GreenFootprintService();

        protected void Page_Load(object sender, EventArgs e)
        {
            //update as needed
        }

        // GreenFootprint

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtRegUsername.Text.Trim();
            bool ok = gfService.RegisterUser(username);
            lblRegisterResult.Text = ok ? "User registered." : "User already exists or error.";
        }

        protected void btnLogGreen_Click(object sender, EventArgs e)
        {
            string username = txtGreenUsername.Text.Trim();
            string action = txtGreenAction.Text.Trim();
            int score = gfService.LogGreenAction(username, action);
            lblGreenResult.Text = "Returned score: " + score;
        }

        protected void btnLogNonGreen_Click(object sender, EventArgs e)
        {
            string username = txtNonGreenUsername.Text.Trim();
            string action = txtNonGreenAction.Text.Trim();
            int score = gfService.LogNonGreenAction(username, action);
            lblNonGreenResult.Text = "Returned score: " + score;
        }

        protected void btnGetScore_Click(object sender, EventArgs e)
        {
            string username = txtScoreUsername.Text.Trim();
            int score = gfService.GetFootprintScore(username);
            lblScoreResult.Text = "Score: " + score;
        }

        protected void btnGetState_Click(object sender, EventArgs e)
        {
            string username = txtStateUsername.Text.Trim();
            string state = gfService.GetGreenState(username);
            lblStateResult.Text = "State: " + state;
        }

        // Weather Service – for now, just echo the URL and input so it compiles

        protected void btnGetWeather_Click(object sender, EventArgs e)
        {
            string zip = txtWeatherZip.Text.Trim();

            txtWeatherOutput.Text =
                "Calling: http://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php\n" +
                "Input zipcode: " + zip + "\n" +
                "Expected Output";
        }

        // Soil Data Service – for now, just echo the URL and input so it compiles

        protected void btnGetSoil_Click(object sender, EventArgs e)
        {
            string query = txtSoilQuery.Text.Trim();

            txtSoilOutput.Text =
                "Calling: https://sdmdataaccess.nrcs.usda.gov/\n" +
                "Input query: " + query + "\n" +
                "Expected Output";
        }
    }
}
