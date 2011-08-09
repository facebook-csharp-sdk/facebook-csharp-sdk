<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CSASPNETWebsiteRegistrationForm.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel runat="server" ID="pnlRegistration" EnableViewState="false">
        <iframe src="<%= this.RegistrationUrl %>"
                scrolling="auto"
                frameborder="no"
                style="border:none"
                allowTransparency="true"
                width="100%"
                height="500">
        </iframe>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlGettingStarted" EnableViewState="false">
        Please set the appropriate values in web.config.<br />
         &lt;facebookSettings appId = "{appid}" appSecret ="{appsecret}" /> <br />
         Also make sure your site url is pointing to the appropriate url in your facebook application settings - http://localhost:5000/
    </asp:Panel>
    </form>
</body>
</html>
