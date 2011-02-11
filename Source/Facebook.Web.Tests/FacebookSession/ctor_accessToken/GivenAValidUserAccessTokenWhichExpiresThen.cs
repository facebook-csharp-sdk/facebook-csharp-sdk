namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using Facebook.Web;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichExpiresThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAValidUserAccessTokenWhichExpiresThen()
        {
            accessToken = "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc";
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
            Assert.Equal("605430316", session.UserId);
        }
    }
}