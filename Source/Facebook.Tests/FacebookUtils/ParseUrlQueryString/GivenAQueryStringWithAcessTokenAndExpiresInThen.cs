namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQueryStringWithAcessTokenAndExpiresInThen
    {
        [Fact]
        public void AccessTokenShouldBeDecodedCorrectly()
        {
            var queryString =
                "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("124973200873702|2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026|ERLPsyFd8CP4ZI57VzAn0nl6WXo", result["access_token"]);
        }

        [Fact]
        public void ExpiresInShouldBeDecodedCorrectly()
        {
            var queryString =
                "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699";

            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal("3699", result["expires_in"]);
        }
    }
}