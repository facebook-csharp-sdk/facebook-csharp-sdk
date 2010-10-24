<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Token.aspx.cs" Inherits="Facebook.Samples.AspWebsite.Token" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="background-color: yellow">
            <fb:login-button></fb:login-button>
            <asp:Label ID="TokenLabel" runat="server"></asp:Label>
        </div>
        <div id="fb-root">
        </div>
        <script src="http://connect.facebook.net/en_US/all.js"></script>
        <script>
            FB.init({ appId: '120625701301347', status: true, cookie: true, xfbml: true });
            FB.Event.subscribe('auth.sessionChange', function (response) {
                if (response.session) {
                    // A user has logged in, and a new cookie has been saved
                } else {
                    // The user has logged out, and the cookie has been cleared
                }
            });
        </script>
    </div>
    </form>
</body>
</html>
