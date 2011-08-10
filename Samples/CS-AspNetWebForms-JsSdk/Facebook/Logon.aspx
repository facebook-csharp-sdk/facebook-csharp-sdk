<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="CS_AspNetWebForms_JsSdk.Facebook.Logon" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms with Facebook JavaScript SDK</title>
    <script type="text/javascript" src="../Scripts/jquery-1.6.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="button" id="fblogin" value="Login to Facebook" disabled="disabled"/>
    <div id="fb-root"></div>
    <script>
        window.fbAsyncInit = function () {
            FB.init({
                appId: '<%: Facebook.FacebookApplication.Current.AppId %>',
                cookie: true,
                xfbml: true,
                oauth: true
            });

            function facebooklogin() {
                FB.login(function (response) {
                    if (response.authResponse) {    
                        // user authorized
                        window.location.reload();
                    } else {
                        // user cancelled
                    }
                }, { scope: '<%: string.Join(",", ExtendedPermissions) %>' });
            };

            $(function () {
                // make the button is only enabled after the facebook js sdk has been loaded.
                $('#fblogin').attr('disabled', false).click(facebooklogin);
            });
        };
        (function () {
            var e = document.createElement('script'); e.async = true;
            e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
            document.getElementById('fb-root').appendChild(e);
        } ());
    </script>
    </form>
</body>
</html>
