namespace Facebook.FacebookClientBase.GetApiUrl
{
    using Tests.FacebookClientBase.GetApiUrl.Fakes;
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