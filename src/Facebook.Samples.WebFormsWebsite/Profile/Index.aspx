<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Facebook.Samples.AspNetWebApplication.Profile.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profile</h2>
    The user is now authenticated with facebook connect.
    <ul>
        <li>Name: <asp:Literal runat="server" ID="name" /></li>
        <li>About: <asp:Literal runat="server" ID="about" /></li>
    </ul>
</asp:Content>
