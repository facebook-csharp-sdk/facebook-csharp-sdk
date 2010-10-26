<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Permissions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Permissions</h2>
    <p>You must grant the following permissions to use this application: <%:ViewData["perms"] %></p>
    <fb:login-button perms="<%:ViewData["perms"] %>"></fb:login-button>
</asp:Content>
