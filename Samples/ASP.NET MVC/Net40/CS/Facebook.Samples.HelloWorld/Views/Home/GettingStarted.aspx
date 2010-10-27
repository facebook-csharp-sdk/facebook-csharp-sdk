<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    GettingStarted
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Getting Started</h2>
    <p>
        You must set up a new Facebook Application before you can use this sample. Go to
        <a href="http://www.facebook.com/developers/createapp.php">http://www.facebook.com/developers/createapp.php</a>
        and create a new applicatoin.</p>
    <p>
        You must set the 'Site URL' property of your Facebook Application to the url of
        this site. If you are running this sample locally, your url will be something like
        http://localhost:1234/.</p>
    <p>
        Read the <a href="http://facebooksdk.codeplex.com/wikipage?title=Getting%20Started&referringTitle=Documentation">
            Getting Started</a> guide on codeplex for more help with configuration.</p>
    <p>
        After you have setup your application, you must configure your web.config file with
        your App ID, Secret, and Key.</p>
    <p>
        After you save your web.config file and refresh or reload this sample, you will
        see a Facebook login button.</p>
</asp:Content>
