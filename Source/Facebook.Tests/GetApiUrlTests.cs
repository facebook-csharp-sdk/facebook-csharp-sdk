namespace Facebook.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class GetApiUrlTests
    {
        [Fact(DisplayName = "GetApiUrl: When method is video.upload The uri should start with api-video facebook domain")]
        public void GetApiUrl_WhenMethodIsVideoUpload_TheUriShouldStartWithApiVideoFacebookDomain()
        {
            var fb = new FakeFacebookApp();

            var uri = fb.GetApiUrl("video.upload");

            Assert.Equal("https://api-video.facebook.com/restserver.php", uri.AbsoluteUri);
        }

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
        [Theory(DisplayName = "GetApiUrl: When method are those of read only The uri should start with api-read facebook domain")]
        public void GetApiUrl_WhenMethodAreThoseOfReadOnly_TheUriShouldStartWithApiReadFacebookDomain(string method)
        {
            var fb = new FakeFacebookApp();

            var uri = fb.GetApiUrl(method);

            Assert.Equal("https://api-read.facebook.com/restserver.php", uri.AbsoluteUri);
        }

        [InlineData("admin.banUsers")]
        [InlineData("admin.getMetrics")]
        [Theory(DisplayName = "GetApiUrl: When method are not read only or video.upload The uri should start with api facebook domain")]
        public void GetApiUrl_WhenMethodAreNotReadOnlyOrVideoUpload_TheUriShouldStartWithApiFacebookDomain(string method)
        {
            var fb = new FakeFacebookApp();

            var uri = fb.GetApiUrl(method);

            Assert.Equal("https://api.facebook.com/restserver.php", uri.AbsoluteUri);
        }

        class FakeFacebookApp : FacebookAppBase
        {
            #region not implemented

            protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi)
            {
                throw new NotImplementedException();
            }

            protected override void RestServerAsync(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void GraphAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void OAuthRequestAsync(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }
            #endregion

            public new System.Uri GetApiUrl(string method)
            {
                return base.GetApiUrl(method);
            }
        }
    }
}