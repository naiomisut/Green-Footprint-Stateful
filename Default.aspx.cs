using System;
using System.Security.Permissions;
using System.Web.UI;
namespace YourNamespace
{
    public partial class Default : System.Web.UI.Page
    {
        GreenFootprintService gfService = new GreenFootprintService();

        protected void Page_Load(object sender, EventArgs e)
        {
            //update as needed
        }

        private String EmojiState(int score)
        {
            if (score < 0) return "User not found";
            else if (score > 0 && score <= 25) return "🌱 Sapling";
            else if (score > 25 && score <= 50) return "🌿 Sprout";
            else if (score > 50 && score <= 75) return "🪴 Plant";
            else return "🌳 Tree";
        }

        // GreenFootprint

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtRegUsername.Text.Trim();
            string password = txtRegPassword.Text.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblRegResult.Text = "Username and password are required.";
                return;
            }
            bool ok = gfService.RegisterUser(username, password);
            lblRegResult.Text = ok ? "Registration successful!" : "Username already exists.";
            if (ok)
            {
                Session["Username"] = username;
            }
        }

        protected void btnLogGreen_Click(object sender, EventArgs e)
        {
            string username = txtGreenUsername.Text.Trim();
            string action = txtGreenAction.Text.Trim();
            int score = gfService.LogGreenAction(username, action);
            string state = gfService.GetGreenState(username);
            lblGreenResult.Text = $"Score {score} | State: {Emojistate}";

            //updates to cookies
            Session["Username"] = username;
            Session["Score"] = score;
            Session["State"] = state;

        }

        protected void btnLogNonGreen_Click(object sender, EventArgs e)
        {
            string username = txtNonGreenUsername.Text.Trim();
            string action = txtNonGreenAction.Text.Trim();
            int score = gfService.LogNonGreenAction(username, action);
            string state = gfService.GetGreenState(username);
            lblNonGreenResult.Text = $"Score {score} | State: {Emojistate}";

            //updates to cookies
            Session["Username"] = username;
            Session["Score"] = score;
            Session["State"] = state;
        }

        protected void btnGetScore_Click(object sender, EventArgs e)
        {
            string username = txtScoreUsername.Text.Trim();
            int score = gfService.GetFootprintScore(username);
            lblScoreResult.Text = "Score: " + score;
            Session["Score"] = score;
        }

        protected void btnGetState_Click(object sender, EventArgs e)
        {
            string username = txtStateUsername.Text.Trim();
            string state = gfService.GetGreenState(username);
            lblStateResult.Text = "State: " + state;
            Session["State"] = state;
        }

        // Weather Service – for now, just echo the URL and input so it compiles

        protected void btnGetWeather_Click(object sender, EventArgs e)
        {
            // Get the zipcode from the textbox and remove extra spaces
            string zip = txtWeatherZip.Text.Trim();

            //txtWeatherOutput.Text =
            //    "Calling: http://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php\n" +
            //    "Input zipcode: " + zip + "\n" +
            //    "Expected Output";

            try
            {
                // Create a client object to talk to the SOAP weather service
                WeatherService.ndfdXMLPortTypeClient client = new WeatherService.ndfdXMLPortTypeClient();

                // Call the service method using the zipcode; should return data from the weather API
                var result = client.LatLonListZipCode(zip);

                // Display the result in the textbox
                txtWeatherOutput.Text = "Zip: " + zip + "\n" + "Weather Service Response:\n" + result;
            }
            catch (Exception ex)
            {
                // When something foes wrong we catch it and throw an error instead of crashing
                txtWeatherOutput.Text = "Error" + ex.Message;
            }
        }

        // Soil Data Service – for now, just echo the URL and input so it compiles

        protected async void btnGetSoil_Click(object sender, EventArgs e)
        {
            // Get user input (our query)
            string query = txtSoilQuery.Text.Trim();

            //txtSoilOutput.Text =
            //    "Calling: https://sdmdataaccess.nrcs.usda.gov/\n" +
            //    "Input query: " + query + "\n" +
            //    "Expected Output";
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    // Need to create an HTTP client to call the REST API
                    string url = "https://sdmdataaccess.nrcs.usda.gov/Tabular/post.rest";

                    // Query must convert to HTTP content (need to send to API)
                    var content = new System.Net.Http.StringContent(query);

                    // Then send a POST request to the API
                    var response = await client.PostAsync(url, content);

                    // Next READ the response returned from the API
                    string result = await response.Content.ReadAsStringAsync();

                    // Lastly DISPLAY the result
                    txtSoilOutput.Text = "Query: " + query + "\n\n" + "Soil Data Response:\n" + result;
                }
            }
            catch (Exception ex)
            {
                //Handle errors safely
                txtSoilOutput.Text = "Error: " + ex.Message;
            }

        }
    }
}
