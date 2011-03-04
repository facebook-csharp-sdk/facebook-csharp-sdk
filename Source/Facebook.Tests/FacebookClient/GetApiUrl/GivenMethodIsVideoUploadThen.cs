namespace Facebook.Tests.FacebookClient.GetApiUrl
{
    using Tests.FacebookClient.GetApiUrl.Fakes;
    using Xunit;

    public class GivenMethodIsVideoUploadThen
    {
        [Fact]
        public void TheUriShouldStartWithApiVideoFacebookDomain()
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl("video.upload");

            Assert.Equal("https://api-video.facebook.com/restserver.php", uri.AbsoluteUri);
        }
    }
}