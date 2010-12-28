<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Facebook C# SDK - Auth and Allow IFrame Sample
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Hello
        <%: ViewData["Firstname"]%>
        <%: ViewData["Lastname"]%>!</h2>
</asp:Content>