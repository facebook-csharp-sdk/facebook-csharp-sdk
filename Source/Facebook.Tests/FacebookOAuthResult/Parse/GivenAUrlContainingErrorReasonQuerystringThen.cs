namespace Facebook.Tests.FacebookOAuthResult.Parse
{
    using Facebook;
    using Xunit;

    public class GivenAUrlContainingErrorReasonQuerystringThen
    {
        [Fact]
        public void TheResultIsUnsuccessful()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookOAuthResult.Parse(url);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void TheErrorReasonTextIsEqualToTheValueOfError_reasonQuerystring()
        {
            var url = "http://www.facebook.com/connect/login_success.html?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookOAuthResult.Parse(url);

            Assert.Equal("user_denied", result.ErrorReason);
        }
    }
}