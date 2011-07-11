
namespace Facebook.Tests.FacebookClient.ctor
{
    using Facebook;
    using Xunit;

    public class CtorWithFacebookApplicationParameter
    {
        [Fact]
        public void GivenNullAccessTokenIsNull()
        {
            var fb = new FacebookClient((IFacebookApplication)null);

            Assert.Null(fb.AccessToken);
        }

        [Fact]
        public void GivenAppIdAndAppSecret_AccessTokenIsNotNull()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId", AppSecret = "appSecret" });

            Assert.NotNull(fb.AccessToken);
        }

        [Fact]
        public void GivenAppIdAndAppSecret_AccessTokenIsCorrectlyBuilt()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId", AppSecret = "appSecret" });

            Assert.Equal("appId|appSecret", fb.AccessToken);
        }

        [Fact]
        public void GivenAppIdButNoAppSecret_AccessTokenIsNull()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId" });

            Assert.Null(fb.AccessToken);
        }

        [Fact]
        public void GivenAppSecretButNoAppId_AccessTokenIsNull()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId" });

            Assert.Null(fb.AccessToken);
        }

        [Fact]
        public void GivenAppIdButEmptyAppSecret_AccessTokenIsNull()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId", AppSecret = string.Empty });

            Assert.Null(fb.AccessToken);
        }

        [Fact]
        public void GivenAppSecretButEmptyAppId_AccessTokenIsNull()
        {
            var fb = new FacebookClient(new DefaultFacebookApplication { AppId = "appId", AppSecret = string.Empty });

            Assert.Null(fb.AccessToken);
        }
    }
}