namespace Facebook.Web.Tests.FacebookSession.ctor_dictionary
{
    using Facebook;
    using Xunit;

    public class GivenAnApplicationAccessTokenOnlyThen
    {
        private readonly FacebookSession session;
        private readonly string accessToken;

        public GivenAnApplicationAccessTokenOnlyThen()
        {
            string appId = "123";
            string appSecret = " A12aB";
            accessToken = string.Format("{0}|{1}", appId, appSecret);

            session = new FacebookSession(new JsonObject { { "access_token", accessToken } });
        }

        [Fact]
        public void AccessTokenShouldBeSetCorrectly()
        {
            Assert.Equal(accessToken, session.AccessToken);
        }

        [Fact]
        public void UserIdShouldBe0()
        {
            Assert.Equal(0, session.UserId);
        }
    }
}