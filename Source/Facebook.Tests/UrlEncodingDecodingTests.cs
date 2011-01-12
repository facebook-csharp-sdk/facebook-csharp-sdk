
namespace Facebook.Tests
{
    using Xunit;
    using Xunit.Extensions;

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

        [InlineData("%7C", "|")]
        [InlineData("3702%7C2", "3702|2")]
        [Theory(DisplayName = "UrlDecode: Given a url with pipe line encoded as %7C Then it should be converted to pipe line symbol |")]
        public void UrlDecode_GivenAUrlWithPipeLineEncodedAsPercent7C_ThenItShouldBeConvertedToPipeLineSymbol(string encodedUrl, string expected)
        {
            var result = FacebookUtils.UrlDecode(encodedUrl);

            Assert.Equal(expected, result);
        }
    }
}