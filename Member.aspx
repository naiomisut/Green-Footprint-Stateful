<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs"
    Inherits="YourNamespace.Member" %>
    <DOCTYPE html>
    <html xmlns=""http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Member Page</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <h2>Welcome to the secured member page</h2>
                <asp: Label ID="lblMessage" runat="server" />
                <br /<>br />
                <asp: Button ID="btnLogout" runat="server" Text="Logout" 
    OnClick="btnLogout_Click" />
            </div>
        </form>
    </body>
    </html>
