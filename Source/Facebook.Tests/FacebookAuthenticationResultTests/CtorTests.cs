namespace Facebook.FacebookAuthenticationResult.Tests
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class CtorTests
    {
        [Fact(DisplayName = "FacebookAuthenticationResult: Given an empty dictionary Then access token should be null")]
        public void FacebookAuthenticationResult_GivenAnEmptyDictionary_ThenAccessTokenShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.AccessToken);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given an empty dictionary Then error reason should be null")]
        public void FacebookAuthenticationResult_GivenAnEmptyDictionary_ThenErrorReasonShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.ErrorReason);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given an empty dictionary Then error description should be null")]
        public void FacebookAuthenticationResult_GivenAnEmptyDictionary_ThenErrorDescriptionShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.ErrorDescription);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given an empty dictionary Then code should be null")]
        public void FacebookAuthenticationResult_GivenAnEmptyDictionary_ThenCodeShouldBeNull()
        {
            var result = new FacebookOAuthResult(new Dictionary<string, object>());

            Assert.Null(result.Code);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given a dictionary with code value Then code should be the one specified in dictionary")]
        public void FacebookAuthenticationResult_GivenADictionaryWithCodeValue_ThenCodeShouldBeTheOneSpecifiedInDictionary()
        {
            var code = "2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0";
            var parameters = new Dictionary<string, object>
                                 {
                                     { "code", code }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.Equal(code, result.Code);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given a dictionary with code and access token Then code should be the one specified in dictionary")]
        public void FacebookAuthenticationResult_GivenADictionaryWithCodeAndAccessToken_ThenCodeShouldBeTheOneSpecifiedInDictionary()
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "code", "code" },
                                    { "access_token", "accesstoken" }
                                };

            var result = new FacebookOAuthResult(parameters);

            Assert.Equal("code", result.Code);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given a dictionary with code Then IsSuccess should be true")]
        public void FacebookAuthenticationResult_GivenADictionaryWithCode_ThenIsSuccessShouldBeTrue()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "code", "dummycode" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given an access token Then IsSuccess should be true")]
        public void FacebookAuthenticationResult_GivenAnAccessToken_ThenIsSuccessShouldBeTrue()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "access_token", "dummy_access_token" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "FacebookAuthenticationResult: Given an error reason Then IsSuccess should be false")]
        public void FacebookAuthenticationResult_GivenAnErrorReason_ThenIsSuccessShouldBeFalse()
        {
            var parameters = new Dictionary<string, object>
                                 {
                                     { "error_reason", "dummy error reason" }
                                 };

            var result = new FacebookOAuthResult(parameters);

            Assert.False(result.IsSuccess);
        }
    }
}