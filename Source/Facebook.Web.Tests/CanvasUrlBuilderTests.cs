/*
namespace Facebook.Web.Tests
{
    using System;
    using Facebook.TestUtils.Fakes;
    using Xunit;

    public class CanvasUrlBuilderTests
    {
        [Fact]
        public void UrlWithCanvasPageSubfolderProducesCorrectUrls()
        {
            var settings = new CanvasSettings();
            settings.CanvasPageUrl = new Uri("http://apps.facebook.com/facebooksdktest");
            settings.CanvasUrl = new Uri("http://www.facebooksdk.net/area/client/controller");
            var request = new FakeHttpRequest(new Uri("http://www.facebooksdk.net/area/client/controller/action"));
            
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request, settings);

            Assert.Equal("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());
            Assert.Equal("http://apps.facebook.com/facebooksdktest", urlBuilder.CanvasPage.ToString());
            Assert.Equal("/facebooksdktest", urlBuilder.CanvasPageApplicationPath);
            Assert.Equal("http://www.facebooksdk.net/area/client/controller", urlBuilder.CanvasUrl.ToString());
            Assert.Equal("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());
            Assert.Equal("/action", urlBuilder.CurrentCanvasPathAndQuery);
            Assert.Equal("http://www.facebooksdk.net/area/client/controller/action", urlBuilder.CurrentCanvasUrl.ToString());
        }

        [Fact]
        public void UrlWithCanvasPageProducesCorrectUrls()
        {
            var settings = new CanvasSettings();
            settings.CanvasPageUrl = new Uri("http://apps.facebook.com/facebooksdktest");
            settings.CanvasUrl = new Uri("http://www.facebooksdk.net");
            var request = new FakeHttpRequest(new Uri("http://www.facebooksdk.net/action"));

            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(request, settings);

           Assert.Equal("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());
           Assert.Equal("http://apps.facebook.com/facebooksdktest", urlBuilder.CanvasPage.ToString());
           Assert.Equal("/facebooksdktest", urlBuilder.CanvasPageApplicationPath);
           Assert.Equal("http://www.facebooksdk.net/", urlBuilder.CanvasUrl.ToString());
           Assert.Equal("http://apps.facebook.com/facebooksdktest/action", urlBuilder.CurrentCanvasPage.ToString());
           Assert.Equal("/action", urlBuilder.CurrentCanvasPathAndQuery);
           Assert.Equal("http://www.facebooksdk.net/action", urlBuilder.CurrentCanvasUrl.ToString());
        }
    }
}
*/