<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_Canvas_AspNetWebForms_JsSdk.Default" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms Canvas Application with Facebook JavaScript SDK</title>
</head>
<body>
    <form id="form1" runat="server">
     <h1>Facebook C# SDK ASP.NET WebForms Canvas Application with Facebook JavaScript SDK</h1>
        <p>This sample demonstrates the use of Facebook C# SDK along with the OAuth 2.0 feature of the Facebook Javascript SDK and ASP.NET WebForms as a Facebook Canvas Application.</p>
        
        <h3>Configuration before running this sample correctly.</h3>
        <ol>
            <li>Create a Facebook application if you haven't at <a href="http://www.facebook.com/developers/createapp.php" target="_blank">http://www.facebook.com/developers/createapp.php</a></li>
            <li>Make sure to set the <strong>Site Url</strong> of the application correctly. (for this sample set it to: <strong><em>http://localhost:1220/</em></strong>). Remember to add the trailing slash at the end of the site url.</li>
            <li>
                Get the AppId and AppSecret and set the appropriate values in web.config file.
                <pre>&lt;facebookSettings appId = "{app_id}" appSecret = "{app_secret}"/&gt;</pre>
            </li>
            <li>Make sure to set the Canvas Page http://apps.facebook.com/<strong>canvaspage</strong>/ and update the web.config file.</li>
            <li>Set the <strong>Canvas Url</strong> to http://localhost:1220/ and update the web.config file.</li>
            <li>Set the <strong>Secure Canvas Url</strong> to https://localhost:44300/ and update the web.config file.</li>
            <li>Refresh this page.</li>
            <li>Now that you have finished configuring the application, navigate to <a target="_top" href="<%= Facebook.FacebookApplication.Current.CanvasPage %>"><%= Facebook.FacebookApplication.Current.CanvasPage %></a></li>
            <li>Then navigate to <a target="_top" href="<%= this.ResolveCanvasPageUrl("~/Facebook/Default.aspx") %>"><%= this.ResolveCanvasPageUrl("~/Facebook/Default.aspx") %></a></li>
        </ol>
    </form>
</body>
</html>
