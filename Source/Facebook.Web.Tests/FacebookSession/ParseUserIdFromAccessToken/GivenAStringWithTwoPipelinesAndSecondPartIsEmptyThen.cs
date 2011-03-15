namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;

    public class GivenAStringWithTwoPipelinesAndSecondPartIsEmptyThen
    {
        [Fact]
        public void TheUserIdShouldBeNull()
        {
            var accessToken = "a||bc";

            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}