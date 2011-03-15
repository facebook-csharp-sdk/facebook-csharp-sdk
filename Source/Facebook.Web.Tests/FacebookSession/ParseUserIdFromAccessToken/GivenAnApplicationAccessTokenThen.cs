namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;

    public class GivenAnApplicationAccessTokenThen
    {
        [Fact]
        public void TheUserIdShouldBeNull()
        {
            string appId = "123";
            string appSecret = " A12aB";

            var userId = FacebookSession.ParseUserIdFromAccessToken(string.Format("{0}|{1}", appId, appSecret));

            Assert.Null(userId);
        }
    }
}