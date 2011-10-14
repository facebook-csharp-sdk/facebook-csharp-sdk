namespace Facebook.Tests.FacebookOAuthClient.GetLoginUrl
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GetLoginUrlResponseTypeEncoding
    {
        [Fact]
        public void GivenParametersAsCodeTokenThenShouldEncodeCorrectly()
        {
            var oauth = new FacebookOAuthClient();

            var loginParameters = new Dictionary<string, object>();
            loginParameters["client_id"] = "appid";
            loginParameters["client_secret"] = "clientsecret";
            loginParameters["response_type"] = "code token";

            var loginUrl = oauth.GetLoginUrl(loginParameters);

            Assert.Equal("http://www.facebook.com/dialog/oauth/?client_id=appid&client_secret=clientsecret&response_type=code%20token&redirect_uri=http%3A%2F%2Fwww.facebook.com%2Fconnect%2Flogin_success.html",
                loginUrl.AbsoluteUri);
        }
    }
}