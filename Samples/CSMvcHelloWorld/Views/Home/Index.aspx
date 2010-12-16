<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
    <p>
        To learn more about the Facebook C# SDK visit <a href="http://facebooksdk.codeplex.com"
            title="Facebook C# SDK">http://facebooksdk.codeplex.com</a>.
    </p>
    <p>
        <img id="fbLogin" src="../../Content/login-button.png" />
    </p>
    <div id="fb-root">
    </div>
    <script src="http://connect.facebook.net/en_US/all.js"></script>
    <script>
        FB.init({ appId: '<%:FacebookSettings.Current.AppId %>', status: true, cookie: true, xfbml: true });
        $('#fbLogin').click(function () {
            FB.login(function (response) {
                if (response.session) {
                    window.location = '<%:Url.Action("About") %>'
                } else {
                    // user cancelled login
                }
            }, { perms: "publish_stream" });
        });
    </script>
</asp:Content>
