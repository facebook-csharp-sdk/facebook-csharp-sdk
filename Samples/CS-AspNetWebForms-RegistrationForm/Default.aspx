<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_AspNetWebForms_RegistrationForm.Default" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</h1>
        <p>This sample demonstrates the use of Facebook C# SDK along with Facebook Registration Form</p>
        
        <h3>Configuration before running this sample correctly.</h3>
        <ol>
            <li>Create a Facebook application if you haven't at <a href="http://www.facebook.com/developers/createapp.php" target="_blank">http://www.facebook.com/developers/createapp.php</a></li>
            <li>Make sure to set the <strong>Site Url</strong> of the application correctly. (for this sample set it to: <strong><em>http://localhost:9242/</em></strong>). Remember to add the trailing slash at the end of the site url.</li>
            <li>
                Get the AppId and AppSecret and set the appropriate values in web.config file.
                <pre>&lt;facebookSettings appId = "{app_id}" appSecret = "{app_secret}"/&gt;</pre>
            </li>
            <li>Now that you have finished configuring the application, navigate to <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Facebook/Register.aspx">http://localhost:9242/Facebook/Register.aspx</asp:HyperLink></li>
        </ol>
    </form>
</body>
</html>
