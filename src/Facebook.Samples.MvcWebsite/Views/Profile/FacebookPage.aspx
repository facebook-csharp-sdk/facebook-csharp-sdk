<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    FacebookPage
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profile</h2>
    Here is info on a facebook page made using the Rest Api.
    <ul>
        <% foreach (var page in Model) { %>
        <li>Name: <%=page.name %></li>
        <% } %>
    </ul>
</asp:Content>
