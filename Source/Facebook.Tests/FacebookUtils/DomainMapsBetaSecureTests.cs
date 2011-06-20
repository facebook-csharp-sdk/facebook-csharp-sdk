namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DomainMapsBetaSecureTests
    {
        [Fact]
        public void CountEquals7()
        {
            var result = FacebookUtils.DomainMapsBetaSecure.Count;

            Assert.Equal(7, result);
        }

        [Fact]
        public void ApiIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_API].ToString();

            Assert.Equal("https://api.beta.facebook.com/", result);
        }

        [Fact]
        public void ApiReadIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_API_READ].ToString();

            Assert.Equal("https://api-read.beta.facebook.com/", result);
        }

        [Fact]
        public void ApiVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_API_VIDEO].ToString();

            Assert.Equal("https://api-video.beta.facebook.com/", result);
        }

        [Fact]
        public void GraphIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_GRAPH].ToString();

            Assert.Equal("https://graph.beta.facebook.com/", result);
        }


        [Fact]
        public void GraphVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_GRAPH_VIDEO].ToString();

            Assert.Equal("https://graph-video.beta.facebook.com/", result);
        }

        [Fact]
        public void WwwIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_WWW].ToString();

            Assert.Equal("https://www.beta.facebook.com/", result);
        }

        [Fact]
        public void AppsIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsBetaSecure[FacebookUtils.DOMAIN_MAP_APPS].ToString();

            Assert.Equal("https://apps.beta.facebook.com/", result);
        }
    }
}