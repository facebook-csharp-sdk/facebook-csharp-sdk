
namespace Facebook.Tests
{
    using System;
    using System.IO;
    using System.Threading;
    using Moq;
    using Moq.Protected;

    public static class TestExtensions
    {
        public static void ReturnsJson(this Mock<Facebook.FacebookClient> facebookClient, string json)
        {
            var mockRequest = new Mock<HttpWebRequestWrapper>();
            var mockResponse = new Mock<HttpWebResponseWrapper>();
            var mockAsyncResult = new Mock<IAsyncResult>();

            var request = mockRequest.Object;
            var response = mockResponse.Object;
            var asyncResult = mockAsyncResult.Object;

            mockRequest.SetupProperty(r => r.Method);
            mockRequest.SetupProperty(r => r.ContentType);
            mockRequest.SetupProperty(r => r.ContentLength);
            mockAsyncResult
                .Setup(ar => ar.AsyncWaitHandle)
                .Returns((ManualResetEvent)null);

            var responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

            mockRequest
                .Setup(r => r.GetResponse())
                .Returns(response);

            AsyncCallback callback = null;

            mockRequest
                .Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Callback<AsyncCallback, object>((c, s) =>
                                                     {
                                                         callback = c;
                                                     })
                .Returns(() =>
                             {
                                 callback(asyncResult);
                                 return asyncResult;
                             });

            mockRequest
                .Setup(r => r.EndGetResponse(It.IsAny<IAsyncResult>()))
                .Returns(response);

            mockResponse
                .Setup(r => r.GetResponseStream())
                .Returns(responseStream);

            mockResponse
                .Setup(r => r.ContentLength)
                .Returns(responseStream.Length);

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri => mockRequest.Setup(r => r.RequestUri).Returns(uri))
                .Returns(request);
        }

        public static void NoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient)
        {
            var mockRequest = new Mock<HttpWebRequestWrapper>();
            var mockWebException = new Mock<WebExceptionWrapper>();
            var mockAsyncResult = new Mock<IAsyncResult>();

            var request = mockRequest.Object;
            var webException = mockWebException.Object;
            var asyncResult = mockAsyncResult.Object;

            mockRequest.SetupProperty(r => r.Method);
            mockRequest.SetupProperty(r => r.ContentType);
            mockRequest.SetupProperty(r => r.ContentLength);
            mockAsyncResult
                .Setup(ar => ar.AsyncWaitHandle)
                .Returns((ManualResetEvent)null);

            mockWebException
                .Setup(e => e.GetResponse())
                .Returns<HttpWebResponseWrapper>(null);

            mockRequest
                .Setup(r => r.GetResponse())
                .Throws(webException);

            mockRequest
                .Setup(r => r.EndGetResponse(It.IsAny<IAsyncResult>()))
                .Throws(webException);

            AsyncCallback callback = null;

            mockRequest
                .Setup(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Callback<AsyncCallback, object>((c, s) =>
                                                     {
                                                         callback = c;
                                                     })
                .Returns(() =>
                             {
                                 callback(asyncResult);
                                 return asyncResult;
                             });

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri =>
                                   {
                                       mockRequest.Setup(r => r.RequestUri).Returns(uri);
                                       mockWebException.Setup(e => e.Message).Returns(string.Format("The remote name could not be resolved: '{0}'", uri.Host));
                                   })
                .Returns(request);
        }

        public static void FiddlerNoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient)
        {
            var mockRequest = new Mock<HttpWebRequestWrapper>();
            var mockResponse = new Mock<HttpWebResponseWrapper>();
            var mockWebException = new Mock<WebExceptionWrapper>();

            var request = mockRequest.Object;
            var response = mockResponse.Object;
            var webException = mockWebException.Object;

            mockRequest.SetupProperty(r => r.Method);
            mockRequest.SetupProperty(r => r.ContentType);
            mockRequest.SetupProperty(r => r.ContentLength);

            var responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Fiddler: DNS Lookup for graph.facebook.com failed.                                                                                                                                                                                                                                                                                                                                                                                                                                                                              "));

            mockResponse
                .Setup(r => r.GetResponseStream())
                .Returns(responseStream);

            mockWebException
                .Setup(e => e.Message)
                .Returns("The remote server returned an error: (502) Bad Gateway.");

            mockWebException
                .Setup(e => e.GetResponse())
                .Returns(response);

            mockRequest
                .Setup(r => r.GetResponse())
                .Throws(webException);

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri => mockRequest.Setup(r => r.RequestUri).Returns(uri))
                .Returns(request);
        }

        public static void Do(Action<ManualResetEvent> callback, Action action, int timeout)
        {
            var evt = new ManualResetEvent(false);

            IAsyncResult resultAction = null;
            IAsyncResult resultCallback = callback.BeginInvoke(evt, ar => resultAction = action.BeginInvoke(ar2 => evt.Set(), null), null);

            if (evt.WaitOne(timeout))
            {
                callback.EndInvoke(resultCallback);
                action.EndInvoke(resultAction);
            }
            else
            {
                throw new TimeoutException();
            }
        }
    }
}