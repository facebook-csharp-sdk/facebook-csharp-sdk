<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CS_AspNetWebForms_RegistrationForm.Facebook.Register" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</title>
</head>
<body>
    <h1>Facebook C# SDK ASP.NET WebForms with Facebook Registration Form</h1>
    <p>This sample demonstrates the use of Facebook C# SDK along with Facebook Registration Form</p>
        
    <form id="form1" runat="server">
         <iframe src="<%= this.RegistrationUrl %>"
                scrolling="auto"
                frameborder="no"
                style="border:none"
                allowTransparency="true"
                width="100%"
                height="500">
        </iframe>
    </form>
</body>
</html>
