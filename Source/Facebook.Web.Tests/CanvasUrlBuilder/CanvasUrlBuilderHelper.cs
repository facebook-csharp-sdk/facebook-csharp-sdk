
namespace Facebook.Web.Tests.CanvasUrlBuilder
{
    using System;
    using System.Collections.Generic;
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
            return new DefaultFacebookApplication { UseFacebookBeta = useBeta };
        }

        public static IEnumerable<object[]> CancelUrlPathRelativeUri
        {
            get
            {
                yield return new object[] { "/cancel" };
                yield return new object[] { "/cancel?name=value" };
                yield return new object[] { "/cancel?" };
                yield return new object[] { "/cancel?name=value&a=b" };
                yield return new object[] { "cancel" };
                yield return new object[] { "cancel?name=value" };
            }
        }

        public static IEnumerable<object[]> CancelUrlPathAbsoluteUri
        {
            get
            {
                yield return new object[] { "http://facebooksdk.codeplex.com/cancel" };
                yield return new object[] { "http://facebooksdk.codeplex.com/cancel?name=value" };
                yield return new object[] { "http://facebooksdk.codeplex.com/cancel?" };
                yield return new object[] { "http://facebooksdk.codeplex.com/cancel?name=value&a=b" };
                yield return new object[] { "http://facebooksdk.codeplex.com" };
                yield return new object[] { "http://facebooksdk.codeplex.coml?name=value" };
            }
        }
    }
}