namespace Facebook.FacebookClientBase.GetApiUrl
{
    using Tests.FacebookClientBase.GetApiUrl.Fakes;
    using Xunit;
    using Xunit.Extensions;

    public class GivenMethodsThanAreNotReadOnlyOrVideoUploadThen
    {
        [InlineData("admin.banUsers")]
        [InlineData("admin.getMetrics")]
        [Theory]
        public void TheUriShouldStartWithApiFacebookDomain(string method)
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl(method);

            Assert.Equal("https://api.facebook.com/restserver.php", uri.AbsoluteUri);
        }
    }
}