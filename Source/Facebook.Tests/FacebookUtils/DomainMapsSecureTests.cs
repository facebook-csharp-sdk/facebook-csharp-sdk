namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DomainMapsSecureTests
    {
        [Fact]
        public void CountEquals7()
        {
            var result = FacebookUtils.DomainMapsSecure.Count;

            Assert.Equal(7, result);
        }

        [Fact]
        public void ApiIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_API].ToString();

            Assert.Equal("https://api.facebook.com/", result);
        }

        [Fact]
        public void ApiReadIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_API_READ].ToString();

            Assert.Equal("https://api-read.facebook.com/", result);
        }

        [Fact]
        public void ApiVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_API_VIDEO].ToString();

            Assert.Equal("https://api-video.facebook.com/", result);
        }

        [Fact]
        public void GraphIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_GRAPH].ToString();

            Assert.Equal("https://graph.facebook.com/", result);
        }

        [Fact]
        public void GraphVideoSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_GRAPH_VIDEO].ToString();

            Assert.Equal("https://graph-video.facebook.com/", result);
        }

        [Fact]
        public void WwwIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_WWW].ToString();

            Assert.Equal("https://www.facebook.com/", result);
        }

        [Fact]
        public void AppsIsSetCorrectly()
        {
            var result = FacebookUtils.DomainMapsSecure[FacebookUtils.DOMAIN_MAP_APPS].ToString();

            Assert.Equal("https://apps.facebook.com/", result);
        }
    }
}