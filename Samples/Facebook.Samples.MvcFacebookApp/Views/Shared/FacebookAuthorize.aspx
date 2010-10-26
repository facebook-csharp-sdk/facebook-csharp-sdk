<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Facebook.Web.Mvc.FacebookAuthorizeInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FacebookAuthorize
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    The following permissions are required: <%=Model.Perms %>
    <h2>FacebookAuthorize</h2>
    <a href="<%=Model.AuthorizeUrl %>" target="_top">Click to Allow This Applicaiton</a>
</asp:Content>
