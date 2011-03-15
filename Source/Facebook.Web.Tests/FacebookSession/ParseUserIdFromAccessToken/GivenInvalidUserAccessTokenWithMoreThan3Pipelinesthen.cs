namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenInvalidUserAccessTokenWithMoreThan2Pipelinesthen
    {
        [Theory]
        [InlineData("two|pipe|lines")]
        [InlineData("three|pipe|line|s")]
        [InlineData("four|pipe|li|ne|s")]
        public void ResultShouldBeNull(string accessToken)
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}