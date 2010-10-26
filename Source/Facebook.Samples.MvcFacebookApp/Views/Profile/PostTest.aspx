<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PostTest
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>PostTest</h2>

    <% Html.BeginForm(new { session = Request.QueryString["session"] }); %>

    <%=Html.TextBox("test") %>
    <input type="submit" name="submit" value="Submit" />
    <% Html.EndForm(); %>

</asp:Content>
