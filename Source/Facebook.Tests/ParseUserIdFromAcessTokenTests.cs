
namespace Facebook.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public class ParseUserIdFromAcessTokenTests
    {
        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token Then the result should not be 0")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessToken_ThenTheResultShouldNotBe0()
        {
            var fb = new FakeFacebookApp();

            var userId =
                fb.ParseUserIdFromAccessToken(
                    "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.NotEqual(0, userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given a valid user access token Then the user id should be correctly extracted from the access token")]
        public void ParseUserIdFromAccessToken_GivenAValidUserAccessToken_ThenTheUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var fb = new FakeFacebookApp();

            var userId =
                fb.ParseUserIdFromAccessToken(
                    "1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            Assert.Equal(605430316, userId);
        }

        [Fact(DisplayName = "ParseUserIdFromAccessToken: Given an application access token Then the user id should be 0")]
        public void ParseUserIdFromAccessToken_GivenAnApplicationAccessToken_ThenTheUserIdShouldBe0()
        {
            var fb = new FakeFacebookApp();
            string appId = "123";
            string appSecret = " A12aB";

            var userId = fb.ParseUserIdFromAccessToken(string.Format("{0}|{1}", appId, appSecret));

            Assert.Equal(0, userId);
        }


        [InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316a|-WE1iH_CV-afTgyhDPc")]
        [InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-as605430316a|-WE1iH_CV-afTgyhDPc")]
        [InlineData("1249203702|2.h1MTNeLqcLqw__.86400.129394400-asfx2|-WE1iH_CV-afTgyhDPc")]
        [Theory(DisplayName = "ParseUserIdFromAccessToken: Given an invalid access token containing not numeric user id Then the user id should be 0")]
        public void ParseUserIdFromAccessToken_GivenAnInvalidAccessTokenContainingNotNumericUserId_ThenTheUserIdShouldBe0(string invalidAccessTokenContainingNonNumbericUserId)
        {
            var fb = new FakeFacebookApp();

            var userId = fb.ParseUserIdFromAccessToken(invalidAccessTokenContainingNonNumbericUserId);

            Assert.Equal(0, userId);
        }

        private class FakeFacebookApp : FacebookAppBase
        {
            #region Overrides of FacebookAppBase

            public override Uri GetLoginUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            protected override void ValidateSessionObject(FacebookSession session)
            {
                throw new NotImplementedException();
            }

            protected override string GenerateSignature(FacebookSession session)
            {
                throw new NotImplementedException();
            }

            protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
            {
                throw new NotImplementedException();
            }

            protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi)
            {
                throw new NotImplementedException();
            }

            protected override void RestServerAsync(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void GraphAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            protected override void OAuthRequestAsync(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            #endregion

            public new long ParseUserIdFromAccessToken(string accessToken)
            {
                return base.ParseUserIdFromAccessToken(accessToken);
            }
        }
    }
}