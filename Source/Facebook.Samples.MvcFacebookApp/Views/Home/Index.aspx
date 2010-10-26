<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
    <p>
        Welcome to your facebook application.
    </p>
    <p>
        <strong>Make sure you change the web.config settings with YOUR app information. The
            facebook app settings in there now are from my development app and will not work
            for you. You can create an app <a href="http://www.facebook.com/developers/createapp.php">here</a>.</strong></p>
            <p>Also, read the getting started guide <a href="http://facebooksdk.codeplex.com/wikipage?title=Getting%20Started&referringTitle=Documentation">here</a>.</p>
</asp:Content>
