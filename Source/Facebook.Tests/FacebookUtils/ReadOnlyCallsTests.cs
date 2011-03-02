namespace Facebook.Tests.FacebookUtils
{
    using System.Linq;
    using Facebook;
    using Xunit;

    public class ReadOnlyCallsTests
    {
        [Fact]
        public void LengthIs60()
        {
            var result = FacebookUtils.ReadOnlyCalls.Length;

            Assert.Equal(60, result);
        }

        [Fact]
        public void ContainsAdminGetAllocation()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getallocation");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetAppProperties()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getappproperties");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetBannedUsers()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getbannedusers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetLiveStreamViaLink()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getlivestreamvialink");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetMetrics()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getmetrics");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetRestrictionInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("admin.getrestrictioninfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsApplicationGetPublicInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("application.getpublicinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetPublicKey()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("auth.getapppublickey");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetSession()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("auth.getsession");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetSignedPublicSessionData()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("auth.getsignedpublicsessiondata");

            Assert.True(result);
        }

        [Fact]
        public void ContainsCommentsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("comments.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGetUnconnectedFriendsCount()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("connect.getunconnectedfriendscount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetActivity()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.getactivity");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetCount()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.getcount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetGlobalNews()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.getglobalnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetNews()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.getnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardMultiGetCount()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.multigetcount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardMultiGetNews()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("dashboard.multigetnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDataGetCookies()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("data.getcookies");

            Assert.True(result);
        }

        [Fact]
        public void ContainsEventsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("events.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsEventsGetMembers()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("events.getmembers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFbmlGetCustomTags()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("fbml.getcustomtags");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetAppFirendStories()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("feed.getappfriendstories");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetrEgisteredTemplateBundleById()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("feed.getregisteredtemplatebundlebyid");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetrEgisteredTemplateBundles()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("feed.getregisteredtemplatebundles");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFqlMultiQuery()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("fql.multiquery");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFqlQuery()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("fql.query");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsAreFriends()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("friends.arefriends");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("friends.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetApUsers()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("friends.getappusers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetLists()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("friends.getlists");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetMultualFriends()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("friends.getmutualfriends");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGiftsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("gifts.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGroupsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("groups.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGroupsGetMembers()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("groups.getmembers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsIntlGetTranslations()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("intl.gettranslations");

            Assert.True(result);
        }

        [Fact]
        public void ContainsLinksGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("links.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsNotesGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("notes.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsNotificationsGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("notifications.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesGetInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("pages.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsAdmin()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("pages.isadmin");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsAppAdded()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("pages.isappadded");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsFan()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("pages.isfan");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPermissionsCheckAvailableApiAccess()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("permissions.checkavailableapiaccess");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPermissionsCheckGrantedApiAccess()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("permissions.checkgrantedapiaccess");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("photos.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGetAlbums()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("photos.getalbums");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGetTags()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("photos.gettags");

            Assert.True(result);
        }

        [Fact]
        public void ContainsProfileGetInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("profile.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsProfileGetInfoOptions()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("profile.getinfooptions");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGet()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("stream.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGetComments()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("stream.getcomments");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGetFilters()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("stream.getfilters");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetLoggedInUser()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.getloggedinuser");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetStandardInfo()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.getstandardinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersHasAppPermission()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.hasapppermission");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersIsAppUser()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.isappuser");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersIsVerified()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("users.isverified");

            Assert.True(result);
        }

        [Fact]
        public void ContainsVideGetUploadLimits()
        {
            var result = FacebookUtils.ReadOnlyCalls.Contains("video.getuploadlimits");

            Assert.True(result);
        }
    }
}