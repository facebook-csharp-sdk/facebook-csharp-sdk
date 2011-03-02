namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DomainMapsBetaTests
    {
        [Fact]
        public void CountEquals5()
        {
            var result = FacebookUtils.DomainMapsBeta.Count;

            Assert.Equal(5, result);
        }

        [Fact]
        public void ApiIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBeta[FacebookUtils.DOMAIN_MAP_API].ToString();

            Assert.Equal("https://api.beta.facebook.com/", result);
        }

        [Fact]
        public void ApiReadIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBeta[FacebookUtils.DOMAIN_MAP_API_READ].ToString();

            Assert.Equal("https://api-read.beta.facebook.com/", result);
        }

        [Fact]
        public void ApiVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBeta[FacebookUtils.DOMAIN_MAP_API_VIDEO].ToString();

            Assert.Equal("https://api-video.beta.facebook.com/", result);
        }

        [Fact]
        public void GraphIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBeta[FacebookUtils.DOMAIN_MAP_GRAPH].ToString();

            Assert.Equal("https://graph.beta.facebook.com/", result);
        }

        [Fact]
        public void WwwIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBeta[FacebookUtils.DOMAIN_MAP_WWW].ToString();

            Assert.Equal("https://www.beta.facebook.com/", result);
        }
    }
}