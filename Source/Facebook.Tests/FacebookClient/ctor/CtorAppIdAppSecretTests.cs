namespace Facebook.Tests.FacebookClient.ctor
{
    using Facebook;
    using Xunit;

    public class CtorAppIdAppSecretTests
    {
        [Fact]
        public void GivenAppIdAndAppSecret_AccessTokenIsSetCorrectly()
        {
            var fb = new FacebookClient("12", "45");

            Assert.Equal("12|45", fb.AccessToken);
        }
    }
}