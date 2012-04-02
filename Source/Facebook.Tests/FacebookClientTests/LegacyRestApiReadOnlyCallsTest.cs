//-----------------------------------------------------------------------
// <copyright file="LegacyRestApiReadOnlyCallsTest.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook.Tests.FacebookClient
{
    using System.Linq;
    using Facebook;
    using Xunit;

    public class LegacyRestApiReadOnlyCallsTest
    {
        [Fact]
        public void LengthIs60()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Length;

            Assert.Equal(60, result);
        }

        [Fact]
        public void ContainsAdminGetAllocation()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getallocation");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetAppProperties()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getappproperties");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetBannedUsers()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getbannedusers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetLiveStreamViaLink()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getlivestreamvialink");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetMetrics()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getmetrics");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAdminGetRestrictionInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("admin.getrestrictioninfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsApplicationGetPublicInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("application.getpublicinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetPublicKey()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("auth.getapppublickey");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetSession()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("auth.getsession");

            Assert.True(result);
        }

        [Fact]
        public void ContainsAuthGetSignedPublicSessionData()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("auth.getsignedpublicsessiondata");

            Assert.True(result);
        }

        [Fact]
        public void ContainsCommentsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("comments.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGetUnconnectedFriendsCount()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("connect.getunconnectedfriendscount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetActivity()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.getactivity");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetCount()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.getcount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetGlobalNews()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.getglobalnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardGetNews()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.getnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardMultiGetCount()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.multigetcount");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDashboardMultiGetNews()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("dashboard.multigetnews");

            Assert.True(result);
        }

        [Fact]
        public void ContainsDataGetCookies()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("data.getcookies");

            Assert.True(result);
        }

        [Fact]
        public void ContainsEventsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("events.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsEventsGetMembers()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("events.getmembers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFbmlGetCustomTags()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("fbml.getcustomtags");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetAppFirendStories()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("feed.getappfriendstories");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetrEgisteredTemplateBundleById()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("feed.getregisteredtemplatebundlebyid");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFeedGetrEgisteredTemplateBundles()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("feed.getregisteredtemplatebundles");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFqlMultiQuery()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("fql.multiquery");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFqlQuery()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("fql.query");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsAreFriends()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("friends.arefriends");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("friends.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetApUsers()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("friends.getappusers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetLists()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("friends.getlists");

            Assert.True(result);
        }

        [Fact]
        public void ContainsFriendsGetMultualFriends()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("friends.getmutualfriends");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGiftsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("gifts.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGroupsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("groups.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsGroupsGetMembers()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("groups.getmembers");

            Assert.True(result);
        }

        [Fact]
        public void ContainsIntlGetTranslations()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("intl.gettranslations");

            Assert.True(result);
        }

        [Fact]
        public void ContainsLinksGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("links.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsNotesGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("notes.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsNotificationsGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("notifications.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesGetInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("pages.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsAdmin()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("pages.isadmin");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsAppAdded()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("pages.isappadded");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPagesIsFan()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("pages.isfan");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPermissionsCheckAvailableApiAccess()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("permissions.checkavailableapiaccess");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPermissionsCheckGrantedApiAccess()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("permissions.checkgrantedapiaccess");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("photos.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGetAlbums()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("photos.getalbums");

            Assert.True(result);
        }

        [Fact]
        public void ContainsPhotosGetTags()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("photos.gettags");

            Assert.True(result);
        }

        [Fact]
        public void ContainsProfileGetInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("profile.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsProfileGetInfoOptions()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("profile.getinfooptions");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGet()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("stream.get");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGetComments()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("stream.getcomments");

            Assert.True(result);
        }

        [Fact]
        public void ContainsStreamGetFilters()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("stream.getfilters");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.getinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetLoggedInUser()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.getloggedinuser");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersGetStandardInfo()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.getstandardinfo");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersHasAppPermission()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.hasapppermission");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersIsAppUser()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.isappuser");

            Assert.True(result);
        }

        [Fact]
        public void ContainsUsersIsVerified()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("users.isverified");

            Assert.True(result);
        }

        [Fact]
        public void ContainsVideGetUploadLimits()
        {
            var result = FacebookClient.LegacyRestApiReadOnlyCalls.Contains("video.getuploadlimits");

            Assert.True(result);
        }
    }
}