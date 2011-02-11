namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using Facebook.Web;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichDoesNotExpireThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAValidUserAccessTokenWhichDoesNotExpireThen()
        {
            accessToken = "1249203702|76a68f298-100001327642026|q_BXv8TmYg";
            session = new FacebookSession(accessToken);
        }
        
        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldNotBeNull()
        {
            Assert.NotNull(session.UserId);
        }

        [Fact]
        public void UserIdShouldBeSetCorrectly()
        {
            Assert.Equal("100001327642026", session.UserId);
        }
    }
}