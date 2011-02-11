namespace Facebook.Tests.FacebookOAuthResult.Parse
{
    using Facebook;
    using Xunit;

    public class GivenErrorDescriptionAsTheUserDeniedYourRequestThen
    {
        [Fact]
        public void TheErrorDescriptionShouldBeConvertedToWhiteSpace()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookOAuthResult.Parse(url);

            Assert.Equal("The user denied your request.", result.ErrorDescription);
        }
    }
}