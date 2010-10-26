<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PagePost
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Publish To Stream</h2>
    <% Html.BeginForm(); %>
    UID: <input type="text" name="uid" /><br />
    Message: <input type="text" name="message" /><br />
    Link Text: <input type="text" name="linkText" /><br />
    Link Url: <input type="text" name="linkUrl" /><br />
    Privacy: <input type="text" name="privacy" /><br />
    Attachment: <input type="file" name="attachment" /><br />
    <input type="submit" value="Publish" />
    <% Html.EndForm(); %>
</asp:Content>