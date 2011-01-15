
namespace Facebook.Tests
{
    using Xunit;

    public class ParseUserIdFromAcessTokenTests
    {
        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token which expires Then the result should not be null")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessTokenWhichExpires_ThenTheResultShouldNotBeNull()
        {
            var userId =
                FacebookSession.ParseUserIdFromAccessToken(
                    "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.NotEqual(null, userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token which does not expire Then the result should not be null")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessTokenWhichDoesNotExpire_ThenTheResultShouldNotBeNull()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|76a68f298-100001327642026|q_BXv8TmYg");

            Assert.NotEqual(null, userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token which expires Then the user id should be correctly extracted from the access token")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessTokenWhichExpires_ThenTheUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var userId =
                FacebookSession.ParseUserIdFromAccessToken(
                    "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.Equal("605430316", userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token which does not expir Then the user id should be correctly extracted from the access token")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessTokenWhichDoesNotExpire_ThenTheUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var userId = FacebookSession.ParseUserIdFromAccessToken("1249203702|76a68f298-100001327642026|q_BXv8TmYg");

            Assert.Equal("100001327642026", userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given an application access token Then the user id should be null")]
        public void ParseUserIdFromAccessToken_GivenAnApplicationAccessToken_ThenTheUserIdShouldBeNull()
        {
            string appId = "123";
            string appSecret = " A12aB";

            var userId = FacebookSession.ParseUserIdFromAccessToken(string.Format("{0}|{1}", appId, appSecret));

            Assert.Null(userId);
        }

        //[InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316a|-WE1iH_CV-afTgyhDPc")]
        //[InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-as605430316a|-WE1iH_CV-afTgyhDPc")]
        //[InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-asfx2|-WE1iH_CV-afTgyhDPc")]
        //[Theory(DisplayName = "ParseUserIdFromAccessToken: Given an invalid access token containing not numeric user id Then the user id should be null")]
        //public void ParseUserIdFromAccessToken_GivenAnInvalidAccessTokenContainingNotNumericUserId_ThenTheUserIdShouldBeNull(string invalidAccessTokenContainingNonNumbericUserId)
        //{
        //    var userId = FacebookSession.ParseUserIdFromAccessToken(invalidAccessTokenContainingNonNumbericUserId);

        //    Assert.Null(userId);
        //}
    }
}