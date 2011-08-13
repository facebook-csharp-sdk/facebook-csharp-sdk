<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_Canvas_AspNetWebForms_WithoutJsSdk.Facebook.Default" %>
<%@ Register TagPrefix="uc1" TagName="SignedRequest" Src="~/Facebook/SignedRequest.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title>Facebook C# SDK ASP.NET WebForms Canvas Application without Facebook JavaScript SDK</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Facebook C# SDK ASP.NET WebForms Canvas Application without Facebook JavaScript SDK</h1>
        <p>This sample demonstrates the use of Facebook C# SDK ASP.NET WebForms along with the OAuth 2.0 feature of Facebook without the Facebook Javascript SDK as a Facebook Canvas Application.</p>
        
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

        <uc1:SignedRequest ID="SignedRequest1" runat="server" />
    </form>
</body>
</html>
