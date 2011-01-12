
namespace Facebook.Tests
{
    using Xunit;

    public class UrlEncodingDecodingTests
    {
        [Fact(DisplayName = "UrlDecode: Given a url part with + Then + should be converted to whitespace")]
        public void UrlDecode_GivenAUrlPartWithPlusSign_ThenPlusSignShouldBeConvertedToWhitespace()
        {
            var urlPart = "The+user+denied+your+request.";

            var result = FacebookUtils.UrlDecode(urlPart);

            Assert.Equal("The user denied your request.", result);
        }

        [Fact(DisplayName = "UrlEncode: Given a url part with white spaces Then white spaces should be converted to +")]
        public void UrlEncode_GivenAUrlPartWithWhiteSpaces_ThenWhiteSpacesShouldBeConvertedToPlusSign()
        {
            var urlPart = "hello world";

            var result = FacebookUtils.UrlEncode(urlPart);

            Assert.Equal("hello+world", result);
        }
    }
}