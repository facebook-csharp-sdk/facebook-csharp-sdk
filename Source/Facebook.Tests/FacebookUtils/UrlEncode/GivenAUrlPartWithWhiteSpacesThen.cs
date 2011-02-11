namespace Facebook.Tests.FacebookUtils.UrlEncode
{
    using Facebook;
    using Xunit;

    public class GivenAUrlPartWithWhiteSpacesThen
    {
        [Fact]
        public void WhiteSpacesShouldBeConvertedToPlusSign()
        {
            var urlPart = "hello world";

            var result = FacebookUtils.UrlEncode(urlPart);

            Assert.Equal("hello+world", result);
        }
    }
}