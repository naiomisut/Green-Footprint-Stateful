using System;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.Security;
using System.Net;
namespace YourNamespace
{
    public partial class Default : System.Web.UI.Page
    {
        GreenFootprintService gfService = new GreenFootprintService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
                imgCaptcha.ImageUrl = "CaptchaImage.aspx?tse=" + DateTime.Now.Ticks; // Unique URL to prevent caching  
            }
        }

        private void GenerateCaptcha()
        {
            // Generate a random number between 1000 and 9999
            Random rand = new Random();
            string code = rand.Next(1000, 9999).ToString();
            // Store the code in the session for later validation
            Session["CaptchaCode"] = code;
        }

        protected void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
            imgCaptcha.ImageUrl = "CaptchaImage.aspx?ts=" + DateTime.Now.Ticks; // Refresh image with unique URL
            lblRegResult.Text = ""; // Clear any previous messages
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
            string enteredCaptcha = txtCaptcha.Text.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblRegResult.Text = "Username and password are required.";
                return;
            }
            if (Session["CaptchaCode"] == null || enteredCaptcha != Session["CaptchaCode"].ToString())
            {
                lblRegResult.Text = "Incorrect CAPTCHA. Please try again.";
                GenerateCaptcha(); // Regenerate CAPTCHA for next attempt
                imgCaptcha.ImageUrl = "CaptchaImage.aspx?ts=" + DateTime.Now.Ticks; // Refresh image
                return;
            }
            bool ok = gfService.RegisterUser(username, password);
            lblRegResult.Text = ok ? "Registration successful!" : "Username already exists.";
            if (ok)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Response.Redirect("Member.aspx");
            }
            else
            {
                GenerateCaptcha(); // Regenerate CAPTCHA for next attempt
                imgCaptcha.ImageUrl = "CaptchaImage.aspx?code=" + DateTime.Now.Ticks; // Refresh image
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text.Trim();
            string password = txtLoginPassword.Text.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblLoginResult.Text = "Username and password are required.";
                return;
            }
            bool valid = gfService.ValidateUser(username, password);
            if (valid)
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Response.Redirect(Member.aspx);
            }
            else
            {
                lblLoginResult.Text = "Invalid username or password.";
            }
        }

        protected void btnLogGreen_Click(object sender, EventArgs e)
        {
            string username = txtGreenUsername.Text.Trim();
            string action = txtGreenAction.Text.Trim();
            int score = gfService.LogGreenAction(username, action);
            string state = gfService.GetGreenState(username);
            lblGreenResult.Text = $"Score {score} | State: {Emojistate(score)}";

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
            lblNonGreenResult.Text = $"Score {score} | State: {Emojistate(score)}";

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
        
        // Staff Login
        protected void btnStaffLogin_Click(object sender, EventArgs e)
        {
            // Gets username input from textbox
            string username = txtStaffUser.Text.Trim();

            // Get password input from textbox
            string password = txtStaffPass.Text.Trim();

            // Check if credentials match the required staff login
            if (username = "TA" && password == "Cse445!")
            {
                // Saves the role in session so we know this user is staff
                Session["role"] = "staff";

                // Redirect to the protected staff page
                Response.Redirect("Staff.aspx");
            }
            else
            {
                // Show error message if login fails
                lblStaffResult.Text = "Invalid Staff Login.";
            }
        }
    }
}
