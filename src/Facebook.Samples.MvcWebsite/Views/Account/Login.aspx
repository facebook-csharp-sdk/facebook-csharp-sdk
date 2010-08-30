<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="Facebook.Web.Mvc.ViewPage<Facebook.Samples.MvcWebApplication.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Log On</h2>
    <p>
       You must connect with your facebook account to view this page.
    </p>

</asp:Content>
