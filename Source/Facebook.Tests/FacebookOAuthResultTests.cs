namespace Facebook.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class FacebookOAuthResultTests
    {
        public class GivenAnEmptyDictionaryThen
        {
            [Fact]
            public void AccessTokenShouldBeNull()
            {
                var result = new FacebookOAuthResult(new Dictionary<string, object>());

                Assert.Null(result.AccessToken);
            }

            [Fact]
            public void ErrorReasonShouldBeNull()
            {
                var result = new FacebookOAuthResult(new Dictionary<string, object>());

                Assert.Null(result.ErrorReason);
            }

            [Fact]
            public void ErrorDescriptionShouldBeNull()
            {
                var result = new FacebookOAuthResult(new Dictionary<string, object>());

                Assert.Null(result.ErrorDescription);
            }

            [Fact]
            public void CodeShouldBeNull()
            {
                var result = new FacebookOAuthResult(new Dictionary<string, object>());

                Assert.Null(result.Code);
            }
        }

        public class GivenADictionaryWithErrorReasonThen
        {
            [Fact]
            public void IsSuccessShouldBeFalse()
            {
                var parameters = new Dictionary<string, object>
                                 {
                                     { "error_reason", "dummy error reason" }
                                 };

                var result = new FacebookOAuthResult(parameters);

                Assert.False(result.IsSuccess);
            }
        }

        public class GivenADictionaryWithCodeValueThen
        {
            [Fact]
            public void CodeShouldBeTheOneSpecifiedInDictionary()
            {
                var code = "2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0";
                var parameters = new Dictionary<string, object>
                                 {
                                     { "code", code }
                                 };

                var result = new FacebookOAuthResult(parameters);

                Assert.Equal(code, result.Code);
            }

            [Fact]
            public void IsSuccessShouldBeTrue()
            {
                var parameters = new Dictionary<string, object>
                                 {
                                     { "code", "dummycode" }
                                 };

                var result = new FacebookOAuthResult(parameters);

                Assert.True(result.IsSuccess);
            }
        }

        public class GivenADictionaryWithCodeAndAccessTokenThen
        {
            [Fact]
            public void CodeShouldBeTheOneSpecifiedInDictionary()
            {
                var parameters = new Dictionary<string, object>
                                {
                                    { "code", "code" },
                                    { "access_token", "accesstoken" }
                                };

                var result = new FacebookOAuthResult(parameters);

                Assert.Equal("code", result.Code);
            }
        }

        public class GivenADictionaryWithAccessTokenThen
        {
            [Fact]
            public void IsSuccessShouldBeTrue()
            {
                var parameters = new Dictionary<string, object>
                                 {
                                     { "access_token", "dummy_access_token" }
                                 };

                var result = new FacebookOAuthResult(parameters);

                Assert.True(result.IsSuccess);
            }
        }
    }
}