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

            var result = FluentHttp.HttpHelper.UrlEncode(urlPart);

            Assert.Equal("hello%20world", result);
        }
    }
}