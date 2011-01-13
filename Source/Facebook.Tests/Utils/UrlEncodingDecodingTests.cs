
namespace Facebook.Utils.Tests
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

        [InlineData("%7c", "|")]
        [InlineData("3702%7c2", "3702|2")]
        [Theory(DisplayName = "UrlEncode: Given a string with piple line symbol Then it should encode piple as %7C")]
        public void UrlEncode_GivenAStringWithPipleLineSymbol_ThenItShouldEncodePipleAsPercent7C(string expectedUrl, string strToEncode)
        {
            var result = FacebookUtils.UrlEncode(strToEncode);

            Assert.Equal(expectedUrl, result);
        }

        [InlineData("124983200973703%7C2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026%7CrPfJfZ38FcwV-8HzRGQdxio9D7B", "124983200973703|2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026|rPfJfZ38FcwV-8HzRGQdxio9D7B")]
        [InlineData("135972300873702%7C3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025%7Cjbo3zk3aHYVJiLWnKArjERsAU0c", "135972300873702|3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025|jbo3zk3aHYVJiLWnKArjERsAU0c")]
        [Theory(DisplayName = "UrlDecode: Given a url encoded facebook access token Then it should decode correctly")]
        public void UrlDecode_GivenAUrlEncodedFacebookAccessToken_ThenItShouldDecodeCorrectly(string encodedAccessToken, string expectedAccessToken)
        {
            var result = FacebookUtils.UrlDecode(encodedAccessToken);

            Assert.Equal(expectedAccessToken, result);
        }
    }
}