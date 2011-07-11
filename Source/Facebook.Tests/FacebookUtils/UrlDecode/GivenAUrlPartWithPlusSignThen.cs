namespace Facebook.Tests.FacebookUtils.UrlDecode
{
    using Facebook;
    using Xunit;

    public class GivenAUrlPartWithPlusSignThen
    {
        [Fact]
        public void PlusSignShouldBeConvertedToWhitespace()
        {
            var urlPart = "The+user+denied+your+request.";

            var result = FluentHttp.HttpHelper.UrlDecode(urlPart);

            Assert.Equal("The user denied your request.", result);
        }
    }
}