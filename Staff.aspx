<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="YourNamespace.Staff" %>
// creation of new page
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Staff Page</title>
</head>
    <body>
        <form id="form1" runat="server">

            <!-- Title for the page so user knows this is staff only -->
            <h2>🔒 Staff Page</h2>

            <!-- Label that will show welcome message after login -->
            <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>
            <br /><br />

            <!-- Button to allow staff to logout and return to main page -->
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />

        </form>
    </body>
</html>