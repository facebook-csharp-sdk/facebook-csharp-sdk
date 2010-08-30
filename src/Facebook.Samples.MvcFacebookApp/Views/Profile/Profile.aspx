<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profile</h2>
    The user is now authenticated with facebook connect.
    <ul>
        <li>Name: <%=Model.name %></li>
        <li>About: <%=Model.about %></li>
    </ul>
</asp:Content>
