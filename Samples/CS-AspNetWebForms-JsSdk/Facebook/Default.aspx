<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CS_AspNetWebForms_JsSdk.Facebook.Default" %>
<!DOCTYPE html>
<html xmlns:fb="https://www.facebook.com/2008/fbml">
<head>
    <title>Facebook C# SDK ASP.NET WebForms with Facebook JavaScript SDK</title>
    <script type="text/javascript" src="../Scripts/jquery-1.6.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <input type="button" id="fblogout" value="Logout from Facebook" disabled="disabled"/>

        <h1>Facebook C# SDK ASP.NET WebForms with Facebook JavaScript SDK</h1>
        <p>This sample demonstrates the use of Facebook C# SDK along with the OAuth 2.0 feature of the Facebook Javascript SDK and ASP.NET WebForms</p>
        
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

        <asp:Label runat="server" AssociatedControlID="txtMessage">Message: </asp:Label><br/>
        <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" style="width:300px;height:100px;" /> <br/>

        <asp:Button runat="server" Text="Post to Wall" ID="btnPostToWall" OnClick="btnPostToWall_Click"/>

        <asp:Label runat="server" ID="lblPostMessageResult" EnableViewState="false"/>

        <div id="fb-root"></div>
        <script>
            window.fbAsyncInit = function () {
                FB.init({
                    appId: '<%: Facebook.FacebookApplication.Current.AppId %>',
                    cookie: true,
                    xfbml: true,
                    oauth: true
                });

                function facebooklogout() {
                    FB.logout(function (response) {
                        // user is now logged out
                        window.location.reload();
                    });
                };

                $(function () {
                    // make the button is only enabled after the facebook js sdk has been loaded.
                    $('#fblogout').attr('disabled', false).click(facebooklogout);
                });
            };
            (function () {
                var e = document.createElement('script'); e.async = true;
                e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
                document.getElementById('fb-root').appendChild(e);
            } ());
        </script>
    </form>
</body>
</html>
