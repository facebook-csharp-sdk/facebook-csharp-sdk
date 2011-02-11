namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAStringWithTwoPipelinesAndSecondPartContainsTwoOrMoreDashesThen
    {
        [Theory]
        [InlineData("a|two-dashe-s|c")]
        [InlineData("a|three-das-he-s|c")]
        public void TheUserIdShouldBeNull(string accessToken)
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}