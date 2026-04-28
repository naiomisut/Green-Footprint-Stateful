<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="GreenFootprintStateful.Deafult" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>GreenFootprint, Weather, Soil TryIt</title>
</head>
<body>
<form id="form1" runat="server">

    <!-- GREEN FOOTPRINT SERVICE -->
    <h1>GreenFootprint Service TryIt</h1>

    <p>
        Tracks a user’s green and non‑green actions, maintaining a score and green state level.
    </p>

    <p>
        Service URL (Localhost):
        <asp:Label ID="lblGFUrl" runat="server"
            Text="http://localhost:12345/GreenFootprint.asmx" />
    </p>

    <ul>
        <li>RegisterUser(string username, string password) : bool</li>
        <li>LogGreenAction(string username, string action) : int</li>
        <li>LogNonGreenAction(string username, string action) : int</li>
        <li>GetFootprintScore(string username) : int</li>
        <li>GetGreenState(string username) : string</li>
    </ul>

    <h3>RegisterUser</h3>
    Username:
    <asp:TextBox ID="txtRegisterUsername" runat="server" />
    <br />
    Password:
    <asp:TextBox ID="txtRegisterPassword" runat="server" TextMode="Password" />
    <br />
    Captcha:
    <br />
    <asp:Image ID="imgCaptcha" runat="server" ImageURL="CaptchaImage.ashx"/>
    <br />
    Enter Captcha:
    <asp:TextBox ID="txtCaptcha" runat="server" />
    <asp:Label ID="lblCaptchaResult" runat="server" ForeColor="Red" />
    <br />
    <asp:Button ID="btnRegister" runat="server" Text="Register"
        OnClick="btnRegister_Click" />
    Result
    <asp:Label ID="lblRegResult" runat="server" />
    <hr />
    
    <h3>Login</h3>
    Username:
    <asp:TextBox ID="txtLoginUsername" runat="server" />
    Password:
    <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" />
    <asp:Button ID="btnLogin" runat="server" Text="Login"
        OnClick="btnLogin_Click" />
    Result:
    <asp:Label ID="lblLoginResult" runat="server" />
    <hr />

    <h3>LogGreenAction</h3>
    Username:
    <asp:TextBox ID="txtGreenUsername" runat="server" />
    Action:
    <asp:TextBox ID="txtGreenAction" runat="server" />
    <asp:Button ID="btnLogGreen" runat="server" Text="Invoke LogGreenAction"
        OnClick="btnLogGreen_Click" />
    New score: <asp:Label ID="lblGreenResult" runat="server" />
    <hr />

    <h3>LogNonGreenAction</h3>
    Username:
    <asp:TextBox ID="txtNonGreenUsername" runat="server" />
    Action:
    <asp:TextBox ID="txtNonGreenAction" runat="server" />
    <asp:Button ID="btnLogNonGreen" runat="server" Text="Invoke LogNonGreenAction"
        OnClick="btnLogNonGreen_Click" />
    New score: <asp:Label ID="lblNonGreenResult" runat="server" />
    <hr />

    <h3>GetFootprintScore</h3>
    Username:
    <asp:TextBox ID="txtScoreUsername" runat="server" />
    <asp:Button ID="btnGetScore" runat="server" Text="Invoke GetFootprintScore"
        OnClick="btnGetScore_Click" />
    Score: <asp:Label ID="lblScoreResult" runat="server" />
    <hr />

    <h3>GetGreenState</h3>
    Username:
    <asp:TextBox ID="txtStateUsername" runat="server" />
    <asp:Button ID="btnGetState" runat="server" Text="Invoke GetGreenState"
        OnClick="btnGetState_Click" />
    State: <asp:Label ID="lblStateResult" runat="server" />
    <hr />

    <!-- WEATHER SERVICE -->
    <h1>Weather Service TryIt</h1>

    <p>
        Uses the National Weather Service NDFD SOAP service to retrieve a 5‑day forecast
        for a U.S. zipcode.
    </p>

    <p>
        Service URL:
        <asp:Label ID="lblWeatherUrl" runat="server"
            Text="http://graphical.weather.gov/xml/SOAP_server/ndfdXMLserver.php" />
    </p>

    <p>
        Operation (example): Get5DayForecast(string zipcode) : string[]
    </p>

    <h3>Get 5‑Day Forecast</h3>
    Zipcode:
    <asp:TextBox ID="txtWeatherZip" runat="server" />
    <asp:Button ID="btnGetWeather" runat="server" Text="Invoke Weather Service"
        OnClick="btnGetWeather_Click" />
    <br />
    Forecast output:
    <asp:TextBox ID="txtWeatherOutput" runat="server"
        TextMode="MultiLine" Rows="6" Columns="60" />
    <hr />

    <!-- SOIL DATA SERVICE -->
    <h1>Soil Data Service TryIt</h1>

    <p>
        Uses the USDA Soil Data Access API to return soil data in JSON format for a given query.
    </p>

    <p>
        Service URL:
        <asp:Label ID="lblSoilUrl" runat="server"
            Text="https://sdmdataaccess.nrcs.usda.gov/" />
    </p>

    <p>
        Operation (example): GetSoilData(string query) : string (JSON)
    </p>

    <h3>Get Soil Data</h3>
    Query:
    <asp:TextBox ID="txtSoilQuery" runat="server" Width="400px" />
    <asp:Button ID="btnGetSoil" runat="server" Text="Invoke Soil Data Service"
        OnClick="btnGetSoil_Click" />
    <br />
    JSON output:
    <asp:TextBox ID="txtSoilOutput" runat="server"
        TextMode="MultiLine" Rows="6" Columns="60" />

    <!-- Staff Login -->
    <p>
        Staff Login
    </p>

    <h3> Staff Login </h3>

    <asp:Textbox ID="txtStaffUser" runat="server" Placeholder="Username"></asp:Textbox>
    <br />

    <asp:Textbox ID="txtStaffPass" runat="server" TextMode="Password" Placeholder="Password"></asp:Textbox>
    <br />

    <asp:Button ID="btnStaffLogin" runat="server" Text="Login" OnClick="btnStaffLogin_Check" />
    <br />

    <asp:Label ID="lblStaffResult" runat="server" Text=""></asp:Label>

</form>
</body>
</html>
