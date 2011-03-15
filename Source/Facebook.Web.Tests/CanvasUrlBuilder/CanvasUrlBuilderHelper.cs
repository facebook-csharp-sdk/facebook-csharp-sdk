
namespace Facebook.Web.Tests.CanvasUrlBuilder
{
    using System;
    using System.Web;
    using Moq;

    public class CanvasUrlBuilderHelper
    {
        public static HttpRequestBase GetFakeHttpRequest(Uri url, Uri urlReferrer)
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.Url).Returns(url);
            requestMock.Setup(request => request.UrlReferrer).Returns(urlReferrer);

            return requestMock.Object;
        }

        public static DefaultFacebookApplication GetFakeFacebookApplication(bool useBeta)
        {
            return new DefaultFacebookApplication {UseFacebookBeta = useBeta};
        }
    }
}