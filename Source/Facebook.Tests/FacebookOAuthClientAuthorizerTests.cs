namespace Facebook.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class FacebookOAuthClientAuthorizerTests
    {
        [Fact(DisplayName = "GetDesktopLoginUri: Given a client id The returned uri should get constructed as Graph Login Uri")]
        public void GetDesktopLoginUri_GivenAClientId_TheReturnedUriShouldGetConstructedAsGraphLoginUri()
        {
            var oauth = new FacebookOAuthClientAuthorizer("123", null, null);

#if WINDOWS_PHONE
            var expectedLoginUri =
                "https://graph.facebook.com/oauth/authorize?client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=touch";
#else
            var expectedLoginUri =
                "https://graph.facebook.com/oauth/authorize?client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=popup";
#endif

            var loginUri = oauth.GetDesktopLoginUri(null);

            Assert.Equal(expectedLoginUri, loginUri.ToString());
        }

        [Fact(DisplayName = "GetDesktopLoginUri: Given a client id and extended permissions as parameters The returned uri should get constructed as Graph Login Uri correctly")]
        public void GetDesktopLoginUri_GivenAClientIdAndExtendedPermissionsAsParameters_TheReturnedUriShouldGetConstructedAsGraphLoginUriCorrectly()
        {
            var oauth = new FacebookOAuthClientAuthorizer("123", null, null);

#if WINDOWS_PHONE
            var expectedLoginUri =
                "https://graph.facebook.com/oauth/authorize?scope=publish_stream,create_event&client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=touch";
#else
            var expectedLoginUri =
                "https://graph.facebook.com/oauth/authorize?scope=publish_stream,create_event&client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=popup";
#endif

            var parameters = new Dictionary<string, object>();
            parameters["scope"] = "publish_stream,create_event";

            var loginUri = oauth.GetDesktopLoginUri(parameters);

            Assert.Equal(expectedLoginUri, loginUri.ToString());
        }
    }
}