namespace Facebook.Tests.FacebookUtils.UrlEncode
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAStringWithPipleLineSymbolThen
    {
        [InlineData("%7c", "|")]
        [InlineData("3702%7c2", "3702|2")]
        [Theory]
        public void ItShouldEncodePipleAsPercent7C(string expectedUrl, string strToEncode)
        {
            var result = FacebookUtils.UrlEncode(strToEncode);

            Assert.Equal(expectedUrl, result);
        }
    }
}