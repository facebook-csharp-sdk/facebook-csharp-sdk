namespace Facebook.Web.Tests.FacebookSession.ctor_accessToken
{
    using Facebook.Web;
    using Xunit;

    public class GivenAnApplicationAccessTokenThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAnApplicationAccessTokenThen()
        {
            string appId = "123";
            string appSecret = " A12aB";
            accessToken = string.Format("{0}|{1}", appId, appSecret);

            session = new FacebookSession(accessToken);
        }

        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldBeNull()
        {
            Assert.Null(session.UserId);
        }
    }
}