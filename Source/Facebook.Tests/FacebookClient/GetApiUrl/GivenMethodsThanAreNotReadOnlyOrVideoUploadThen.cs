namespace Facebook.Tests.FacebookClient.GetApiUrl
{
    using Tests.FacebookClient.GetApiUrl.Fakes;
    using Xunit;
    using Xunit.Extensions;

    public class GivenMethodsThanAreNotReadOnlyOrVideoUploadThen
    {
        [InlineData("admin.banUsers")]
        [Theory]
        public void TheUriShouldStartWithApiFacebookDomain(string method)
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl(method);

            Assert.Equal("https://api.facebook.com/restserver.php", uri.AbsoluteUri);
        }
    }
}