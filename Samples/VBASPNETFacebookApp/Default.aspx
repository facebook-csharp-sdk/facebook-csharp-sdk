<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="pnlHello" runat="server" Visible="false">
        <h2>
            Hello
            <asp:Label ID="lblName" runat="server" />!
        </h2>
    </asp:Panel>
    <asp:Panel ID="pnlError" runat="server" Visible="false">
        <a href="Default.aspx">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" /><br />
        </a>
    </asp:Panel>
</asp:Content>