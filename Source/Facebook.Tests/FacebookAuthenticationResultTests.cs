namespace Facebook.Tests
{
    using Xunit;

    public class FacebookAuthenticationResultTests
    {
        [Fact(DisplayName = "Parse: Given a url containing access_token and expires_in fragment The result is successful")]
        public void Parse_GivenAUrlContainingAccessTokenAndExpiresInFragment_TheResultIsSuccessful()
        {
            var url = "http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "Parse: Given a url containing access_token and expires in fragment The access token of result is equal to the value passed in the access_token querystring")]
        public void Parse_GivenAUrlContainingAccessTokenAndExpiresInFragment_TheAccessTokenOfResultIsEqualToTheValuePassedInTheAccessTokenQuerystring()
        {
            var url = "http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.Equal("123|654aaaee068db-100001327642026|sd", result.AccessToken);
        }

        [Fact(DisplayName = "Parse: Given a url containing access_token and expires in fragment The ExpiresIn of result is equal to the value passed in the expires_in querystring")]
        public void Parse_GivenAUrlContainingAccess_tokenAndExpiresInFragment_TheExpiresInOfResultIsEqualToTheValuePassedInTheExpiresInQuerystring()
        {
            var url = "http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0";

            var result = FacebookAuthenticationResult.Parse(url);

            var expiresIn = FacebookUtils.ToUnixTime(result.Expires);
            Assert.Equal(0, expiresIn);
        }

        [Fact(DisplayName = "Parse: Given a url containing access_token and expires in fragment Then ErrorReasonText is null")]
        public void Parse_GivenAUrlContainingAccess_tokenAndExpiresInFragment_ThenErrorReasonTextIsNull()
        {
            var url = "http://www.facebook.com/connect/login_success.html#access_token=123|654aaaee068db-100001327642026|sd&expires_in=0";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.Null(result.ErrorReason);
        }

        [Fact(DisplayName = "Parse: Given a url containing error_reason querystring Then the result is unsuccessful")]
        public void Parse_GivenAUrlContainingErrorReasonQuerystring_ThenTheResultIsUnsuccessful()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.False(result.IsSuccess);
        }

        [Fact(DisplayName = "Parse: Given a url containing error_reason querystring Then the ErrorReasonText is equal to the value of error_reason querystring")]
        public void Parse_GivenAUrlContainingErrorReasonQuerystring_ThenTheErrorReasonTextIsEqualToTheValueOfError_reasonQuerystring()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.Equal("user_denied", result.ErrorReason);
        }

        [Fact(DisplayName = "Parse: Given error_description as The+user+denied+your+request. Then the error description's + should be converted to white space.")]
        public void Parse_GivenErrorDescriptionAsTheUserDeniedYourRequest_ThenTheErrorDescriptionShouldBeConvertedToWhiteSpace()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.Equal("The user denied your request.", result.ErrorDescription);
        }

        [Fact(DisplayName = "Parse: Given error_reason as user_denied Then the error reason should be the same")]
        public void Parse_GivenErrorReasonAsUserDenied_ThenTheErrorReasonShouldBeTheSame()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookAuthenticationResult.Parse(url);

            Assert.Equal("user_denied", result.ErrorReason);
        }
    }
}