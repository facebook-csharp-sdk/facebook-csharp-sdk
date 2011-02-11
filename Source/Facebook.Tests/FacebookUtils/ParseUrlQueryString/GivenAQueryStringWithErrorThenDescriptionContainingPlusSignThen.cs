namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQueryStringWithErrorThenDescriptionContainingPlusSignThen
    {
        [Fact]
        public void Error_descriptionShouldBeDecodedCorrectly()
        {
            var queryString = "error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("The user denied your request.", result["error_description"]);
        }
    }
}