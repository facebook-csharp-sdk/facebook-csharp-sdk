<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Connect.aspx.cs" Inherits="Facebook.Samples.AspWebsite.Connect" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div>
            <img src="/Images/login-button.png" id="fbLogin" />
        </div>
        <div id="fb-root">
        </div>
        <script src="http://connect.facebook.net/en_US/all.js"></script>
        <script>
            FB.init({ appId: '120625701301347', status: true, cookie: true, xfbml: true });
            $('#fbLogin').click(function () {
                FB.login(function (response) {
                    if (response.session) {
                        window.location = '<%: Request.QueryString["returnUrl"] ?? "/" %>';
                    } else {
                        // user cancelled login
                    }
                });
            });
        </script>
</asp:Content>
