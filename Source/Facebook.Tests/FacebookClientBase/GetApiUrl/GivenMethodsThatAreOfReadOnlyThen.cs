namespace Facebook.Tests.FacebookClient.GetApiUrl
{
    using Tests.FacebookClient.GetApiUrl.Fakes;
    using Xunit;
    using Xunit.Extensions;

    public class GivenMethodsThatAreOfReadOnlyThen
    {
        [InlineData("admin.getallocation")]
        [InlineData("admin.getappproperties")]
        [InlineData("admin.getbannedusers")]
        [InlineData("admin.getlivestreamvialink")]
        [InlineData("admin.getmetrics")]
        [InlineData("admin.getrestrictioninfo")]
        [InlineData("application.getpublicinfo")]
        [InlineData("auth.getapppublickey")]
        [InlineData("auth.getsession")]
        [InlineData("auth.getsignedpublicsessiondata")]
        [InlineData("comments.get")]
        [InlineData("connect.getunconnectedfriendscount")]
        [InlineData("dashboard.getactivity")]
        [InlineData("dashboard.getcount")]
        [InlineData("dashboard.getglobalnews")]
        [InlineData("dashboard.getnews")]
        [InlineData("dashboard.multigetcount")]
        [InlineData("dashboard.multigetnews")]
        [InlineData("data.getcookies")]
        [InlineData("events.get")]
        [InlineData("events.getmembers")]
        [InlineData("fbml.getcustomtags")]
        [InlineData("feed.getappfriendstories")]
        [InlineData("feed.getregisteredtemplatebundlebyid")]
        [InlineData("feed.getregisteredtemplatebundles")]
        [InlineData("fql.multiquery")]
        [InlineData("fql.query")]
        [InlineData("friends.arefriends")]
        [InlineData("friends.get")]
        [InlineData("friends.getappusers")]
        [InlineData("friends.getlists")]
        [InlineData("friends.getmutualfriends")]
        [InlineData("gifts.get")]
        [InlineData("groups.get")]
        [InlineData("groups.getmembers")]
        [InlineData("intl.gettranslations")]
        [InlineData("links.get")]
        [InlineData("notes.get")]
        [InlineData("notifications.get")]
        [InlineData("pages.getinfo")]
        [InlineData("pages.isadmin")]
        [InlineData("pages.isappadded")]
        [InlineData("pages.isfan")]
        [InlineData("permissions.checkavailableapiaccess")]
        [InlineData("permissions.checkgrantedapiaccess")]
        [InlineData("photos.get")]
        [InlineData("photos.getalbums")]
        [InlineData("photos.gettags")]
        [InlineData("profile.getinfo")]
        [InlineData("profile.getinfooptions")]
        [InlineData("stream.get")]
        [InlineData("stream.getcomments")]
        [InlineData("stream.getfilters")]
        [InlineData("users.getinfo")]
        [InlineData("users.getloggedinuser")]
        [InlineData("users.getstandardinfo")]
        [InlineData("users.hasapppermission")]
        [InlineData("users.isappuser")]
        [InlineData("users.isverified")]
        [InlineData("video.getuploadlimits")]
        [Theory]
        public void TheUriShouldStartWithApiReadFacebookDomain(string method)
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl(method);

            Assert.Equal("https://api-read.facebook.com/restserver.php", uri.AbsoluteUri);
        }
    }
}