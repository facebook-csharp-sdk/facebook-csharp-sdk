namespace Facebook.Tests._fb
{
    using System;
    using Facebook;
    using Xunit;

    public class ParseOAuthCallbackUrlTests
    {
        public class GivenAUrlContainingAccessTokenAndExpiresInFragmentThen
        {
            private readonly FacebookClient _fb;

            public GivenAUrlContainingAccessTokenAndExpiresInFragmentThen()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void TheResultIsSuccessful()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.True(result.IsSuccess);
            }

            [Fact]
            public void TheAccessTokenOfResultIsEqualToTheValuePassedInTheAccessTokenQuerystring()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Equal("123|654aaaee068db-100001327642026|sd", result.AccessToken);
            }

            [Fact]
            public void IfExpiresIs0ThenTheResultOfExpiresIsMaxDateTime()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0");

                var result = _fb.ParseOAuthCallbackUrl(url);

                var expiresIn = result.Expires;
                Assert.Equal(DateTime.MaxValue, expiresIn);
            }

            [Fact]
            public void ErrorReasonTextIsNull()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Null(result.ErrorReason);
            }
        }

        public class GivenAUrlContainingErrorReasonQuerystringThen
        {
            private readonly FacebookClient _fb;

            public GivenAUrlContainingErrorReasonQuerystringThen()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void TheResultIsUnsuccessful()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.False(result.IsSuccess);
            }

            [Fact]
            public void TheErrorReasonTextIsEqualToTheValueOfError_reasonQuerystring()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Equal("user_denied", result.ErrorReason);
            }
        }

        public class GivenCodeThen
        {
            private readonly FacebookClient _fb;

            public GivenCodeThen()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void TheCodeShouldBeTheSame()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html?code=2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Equal("2.XeyH7lWz33itx1R86_uBeg__.3600.1294930800-100001327642026|t8SsfSR2XI6yhBAkhX95J7p9hJ0", result.Code);
            }
        }

        public class GivenErrorDescriptionAsTheUserDeniedYourRequestThen
        {
            private readonly FacebookClient _fb;

            public GivenErrorDescriptionAsTheUserDeniedYourRequestThen()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void TheErrorDescriptionShouldBeConvertedToWhiteSpace()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Equal("The user denied your request.", result.ErrorDescription);
            }
        }

        public class GivenErrorReasonAsUserDeniedThen
        {
            private readonly FacebookClient _fb;

            public GivenErrorReasonAsUserDeniedThen()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void TheErrorReasonShouldBeTheSame()
            {
                var url = new Uri("http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.");

                var result = _fb.ParseOAuthCallbackUrl(url);

                Assert.Equal("user_denied", result.ErrorReason);
            }
        }
    }
}