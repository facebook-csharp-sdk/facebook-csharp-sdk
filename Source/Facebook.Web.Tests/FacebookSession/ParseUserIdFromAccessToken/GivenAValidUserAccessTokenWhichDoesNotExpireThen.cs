namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichDoesNotExpireThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|76a68f298-100001327642026|q_BXv8TmYg");

            Assert.NotEqual(null, userId);
        }

        [Fact]
        public void TheUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|76a68f298-100001327642026|q_BXv8TmYg");

            Assert.Equal("100001327642026", userId);
        }
    }
}