
namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DomainMapConstansTests
    {
        [Fact]
        public void DomainMapApiEqualsApi()
        {
            var result = FacebookUtils.DOMAIN_MAP_API;

            Assert.Equal("api", result);
        }

        [Fact]
        public void DomainMapApiReadIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_API_READ;

            Assert.Equal("api_read", result);
        }

        [Fact]
        public void DomainMapApiVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_API_VIDEO;

            Assert.Equal("api_video", result);
        }

        [Fact]
        public void DomainMapGraphIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_GRAPH;

            Assert.Equal("graph", result);
        }

        [Fact]
        public void DomainMapGraphVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_GRAPH_VIDEO;

            Assert.Equal("graph_video", result);
        }

        [Fact]
        public void DomainMapWwwIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_WWW;

            Assert.Equal("www", result);
        }

        [Fact]
        public void DomainMapAppsIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_APPS;

            Assert.Equal("apps", result);
        }
    }
}