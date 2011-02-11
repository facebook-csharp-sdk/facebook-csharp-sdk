namespace Facebook.Tests.FacebookUtils.RemoveTrailingSlash
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlWithTrailingSlashAndNoQuerystringThen
    {
        [InlineData("http://www.ntotten.com/", "http://www.ntotten.com")]
        [InlineData("jimzimmerman.com/", "jimzimmerman.com")]
        [InlineData("www.prabir.me/", "www.prabir.me")]
        [Theory]
        public void ItShouldRemoveTheTrailingSlash(string inputUrl, string expectedUrl)
        {
            var result = FacebookUtils.RemoveTrailingSlash(inputUrl);

            Assert.Equal(expectedUrl, result);
        }
    }
}