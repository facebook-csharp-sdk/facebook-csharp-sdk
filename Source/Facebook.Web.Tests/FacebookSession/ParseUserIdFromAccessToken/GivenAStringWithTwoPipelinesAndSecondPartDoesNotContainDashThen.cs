namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;

    public class GivenAStringWithTwoPipelinesAndSecondPartDoesNotContainDashThen
    {
        [Fact]
        public void TheUserIdShouldBeNull()
        {
            var accessToken = "a|no_dash|bc";

            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}