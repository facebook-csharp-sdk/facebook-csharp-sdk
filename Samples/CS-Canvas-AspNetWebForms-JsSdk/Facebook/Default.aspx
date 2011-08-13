<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_Canvas_AspNetWebForms_JsSdk.Facebook.Default" %>

<%@ Register src="FacebookLoginControl.ascx" tagname="FacebookLoginControl" tagprefix="uc1" %>

<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms Canvas Application with Facebook JavaScript SDK</title>
    <script type="text/javascript" src="../Scripts/jquery-1.6.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">    
        <h1>Facebook C# SDK ASP.NET WebForms Canvas Application with Facebook JavaScript SDK</h1>
        <p>This sample demonstrates the use of Facebook C# SDK along with the OAuth 2.0 feature of the Facebook Javascript SDK and ASP.NET WebForms as a Facebook Canvas Application.</p>

        <% if (IsAuthorized) { %>
            <strong>Hi <asp:Label runat="server" ID="lblName"></asp:Label></strong> <br/>
            
            <asp:Image runat="server" ID="imgProfilePic"/>

            <table>
                <tr>
                    <td>First Name:</td>
                    <td><asp:label runat="server" ID="lblFirstName"/></td>
                </tr>
                <tr>
                    <td>Last Name:</td>
                    <td><asp:label runat="server" ID="lblLastName"/></td>
                </tr>
            </table>

            <br/>

            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtMessage">Message: </asp:Label><br/>
            <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" style="width:300px;height:100px;" /> <br/>

            <asp:Button runat="server" Text="Post to Wall" ID="btnPostToWall" OnClick="btnPostToWall_Click"/>

            <asp:Label runat="server" ID="lblPostMessageResult" EnableViewState="false"/>

        <% } else { %>
            <uc1:FacebookLoginControl ID="FacebookLoginControl1" runat="server" />
        <% } %>
    </form>
</body>
</html>
