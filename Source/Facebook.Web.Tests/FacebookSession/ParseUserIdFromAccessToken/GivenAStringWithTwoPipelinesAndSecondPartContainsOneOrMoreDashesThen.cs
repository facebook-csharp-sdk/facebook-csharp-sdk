namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAStringWithTwoPipelinesAndSecondPartContainsOneOrMoreDashesThen
    {
        [Theory]
        [InlineData("a|one-1|c", "1")]
        [InlineData("a|two-dashe-1|c", "1")]
        [InlineData("a|three-das-he-23|c", "23")]
        public void TheUserIdShouldNotBeNullIfTheLastIsNotEmpty(string accessToken, string id)
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.NotNull(userId);
            Assert.Equal(id, userId);
        }

        [Theory]
        [InlineData("a|one-|c")]
        [InlineData("a|two-dashe-|c")]
        [InlineData("a|three-das-he-|c")]
        public void TheUserIdShouldBeNullIfTheLastIsEmpty(string accessToken)
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}