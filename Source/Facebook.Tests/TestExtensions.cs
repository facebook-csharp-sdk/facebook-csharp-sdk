//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

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
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            facebookClient.ReturnsJson(json, out mockRequest, out mockResponse);
        }

        public static void ReturnsJson(this Mock<Facebook.FacebookClient> facebookClient, string json, out Mock<HttpWebRequestWrapper> mockRequest, out Mock<HttpWebResponseWrapper> mockResponse)
        {
            mockRequest = new Mock<HttpWebRequestWrapper>();
            mockResponse = new Mock<HttpWebResponseWrapper>();
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

            var mockRequestCopy = mockRequest;

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri => mockRequestCopy.Setup(r => r.RequestUri).Returns(uri))
                .Returns(request);
        }

        public static void NoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient)
        {
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            facebookClient.NoInternetConnection(out mockRequest, out mockWebException);
        }

        public static void NoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient, out Mock<HttpWebRequestWrapper> mockRequest, out Mock<WebExceptionWrapper> mockWebException)
        {
            mockRequest = new Mock<HttpWebRequestWrapper>();
            mockWebException = new Mock<WebExceptionWrapper>();
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

            var mockRequestCopy = mockRequest;
            var mockWebExceptionCopy = mockWebException;

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri =>
                                   {
                                       mockRequestCopy.Setup(r => r.RequestUri).Returns(uri);
                                       mockWebExceptionCopy.Setup(e => e.Message).Returns(string.Format("The remote name could not be resolved: '{0}'", uri.Host));
                                   })
                .Returns(request);
        }

        public static void FiddlerNoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient)
        {
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            facebookClient.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);
        }

        public static void FiddlerNoInternetConnection(this Mock<Facebook.FacebookClient> facebookClient, out Mock<HttpWebRequestWrapper> mockRequest, out Mock<HttpWebResponseWrapper> mockResponse, out Mock<WebExceptionWrapper> mockWebException)
        {
            mockRequest = new Mock<HttpWebRequestWrapper>();
            mockResponse = new Mock<HttpWebResponseWrapper>();
            mockWebException = new Mock<WebExceptionWrapper>();

            var mockAsyncResult = new Mock<IAsyncResult>();

            var request = mockRequest.Object;
            var response = mockResponse.Object;
            var webException = mockWebException.Object;
            var asyncResult = mockAsyncResult.Object;

            mockRequest.SetupProperty(r => r.Method);
            mockRequest.SetupProperty(r => r.ContentType);
            mockRequest.SetupProperty(r => r.ContentLength);
            mockAsyncResult
               .Setup(ar => ar.AsyncWaitHandle)
               .Returns((ManualResetEvent)null);

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

            var mockRequestCopy = mockRequest;

            facebookClient.Protected()
                .Setup<HttpWebRequestWrapper>("CreateHttpWebRequest", ItExpr.IsAny<Uri>())
                .Callback<Uri>(uri => mockRequestCopy.Setup(r => r.RequestUri).Returns(uri))
                .Returns(request);
        }

        public static void VerifyGetResponse(this Mock<HttpWebRequestWrapper> mockRequest)
        {
            mockRequest.Verify(r => r.GetResponse());
        }

        public static void VerifyBeginGetResponse(this Mock<HttpWebRequestWrapper> mockRequest)
        {
            mockRequest.Verify(r => r.BeginGetResponse(It.IsAny<AsyncCallback>(), It.IsAny<object>()));
        }

        public static void VerifyEndGetResponse(this Mock<HttpWebRequestWrapper> mockRequest)
        {
            mockRequest.Verify(r => r.EndGetResponse(It.IsAny<IAsyncResult>()));
        }

        public static void VerifyGetReponse(this Mock<WebExceptionWrapper> mockWebException)
        {
            mockWebException.Verify(e => e.GetResponse());
        }

        public static void VerifyGetResponseStream(this Mock<HttpWebResponseWrapper> mockResponse)
        {
            mockResponse.Verify(r => r.GetResponseStream());
        }

        public static void VerifyCreateHttpWebRequest(this Mock<Facebook.FacebookClient> mockFb, Times times)
        {
            mockFb
                .Protected()
                .Verify("CreateHttpWebRequest", times, ItExpr.IsAny<Uri>());
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