<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="CSASPNETWebsite.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Log In
    </h2>
    <p>
        <fb:login-button></fb:login-button>
    </p>
    <div id="fb-root">
    </div>
    <script src="http://connect.facebook.net/en_US/all.js"></script>
    <script>
        FB.init({ appId: '<%: Facebook.FacebookSettings.Current.AppId %>', status: true, cookie: true, xfbml: true });
        FB.Event.subscribe('auth.sessionChange', function (response) {
            if (response.session) {
                // A user has logged in, and a new cookie has been saved
                window.location.reload();
            } else {
                // The user has logged out, and the cookie has been cleared
            }
        });
    </script>
</asp:Content>
