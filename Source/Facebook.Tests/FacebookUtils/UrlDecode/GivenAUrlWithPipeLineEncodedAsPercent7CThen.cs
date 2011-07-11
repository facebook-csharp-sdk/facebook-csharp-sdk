namespace Facebook.Tests.FacebookUtils.UrlDecode
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlWithPipeLineEncodedAsPercent7CThen
    {
        [InlineData("%7C", "|")]
        [InlineData("3702%7C2", "3702|2")]
        [Theory]
        public void ItShouldBeConvertedToPipeLineSymbol(string encodedUrl, string expected)
        {
            var result = FluentHttp.HttpHelper.UrlDecode(encodedUrl);

            Assert.Equal(expected, result);
        }
    }
}