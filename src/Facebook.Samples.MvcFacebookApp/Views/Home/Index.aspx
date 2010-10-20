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
            for you.</strong></p>
</asp:Content>
