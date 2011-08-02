namespace Facebook.Tests.FacebookClient.GetApiUrl
{
    using Tests.FacebookClient.GetApiUrl.Fakes;
    using Xunit;

    public class GivenMethodIsNotAllSmallLetterThen
    {
        [Fact]
        public void VideoUploadTests()
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl("video.Upload");

            Assert.Equal("https://api-video.facebook.com/restserver.php", uri.AbsoluteUri);
        }

        [Fact]
        public void ReadOnlyApiCalls()
        {
            var fb = new FakeFacebookClient();

            var uri = fb.GetApiUrl("users.GetStandardinfo");

            Assert.Equal("https://api-read.facebook.com/restserver.php", uri.AbsoluteUri);
        }
    }
}