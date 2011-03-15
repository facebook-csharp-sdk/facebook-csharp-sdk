namespace Facebook.Web.Tests.FacebookSession.ParseUserIdFromAccessToken
{
    using Facebook;
    using Xunit;

    public class GivenAValidUserAccessTokenWhichExpiresThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.NotEqual(null, userId);
        }

        [Fact]
        public void TheUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.Equal("605430316", userId);
        }
    }
}