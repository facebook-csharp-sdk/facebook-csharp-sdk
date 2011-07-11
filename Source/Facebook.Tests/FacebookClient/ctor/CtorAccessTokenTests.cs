namespace Facebook.Tests.FacebookClient.ctor
{
    using Facebook;
    using Xunit;

    public class CtorAccessTokenTests
    {
        [Fact]
        public void GivenAccessToken_AccessTokenIsNotNull()
        {
            var fb = new FacebookClient("access_token");

            Assert.NotNull(fb.AccessToken);
        }

        [Fact]
        public void GivenAccessToken_AccessTokenIsSetCorrectly()
        {
            var fb = new FacebookClient("access_token");

            Assert.Equal("access_token", fb.AccessToken);
        }
    }
}