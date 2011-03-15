namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenInvalidUserAccessTokenLessThan2PipelinesThen
    {
        [Theory]
        [InlineData("no_pipeline")]
        [InlineData("one|pipeline")]
        public void ResultShouldBeNull(string accessToken)
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken(accessToken);

            Assert.Null(userId);
        }
    }
}