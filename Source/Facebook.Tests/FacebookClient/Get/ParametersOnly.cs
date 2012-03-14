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

namespace Facebook.Tests.FacebookClient.Get
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Moq;
    using Xunit;

    public class ParametersOnly
    {
        private Dictionary<string, object> _parameters;

        public ParametersOnly()
        {
            _parameters = new Dictionary<string, object>
                                 {
                                     {"method", "users.getInfo"},
                                     {"uids", new[] {4}},
                                     {"fields", new[] {"first_name", "last_name", "name"}}
                                 };
        }

        [Fact]
        public void Sync_DoesNotCallGetCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            fb.GetCompleted += (o, e) => ++called;

            fb.Get(_parameters);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void Sync_DoesNotCallPostCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            fb.PostCompleted += (o, e) => ++called;

            fb.Get(_parameters);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void Sync_DoesNotCallDeleteCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            fb.DeleteCompleted += (o, e) => ++called;

            fb.Get(_parameters);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void SyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_ThrowsWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            var fb = mockFb.Object;

            Exception exception = null;

            try
            {
                fb.Get(_parameters);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.IsAssignableFrom<WebExceptionWrapper>(exception);
        }

        [Fact]
        public void SyncWhenThereIsNotInternetConnectionAndFiddlerIsNotOpen_ThrowsWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            Exception exception = null;

            try
            {
                var fb = mockFb.Object;
                fb.Get(_parameters);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.IsAssignableFrom<WebExceptionWrapper>(exception);
        }

        [Fact]
        public void SyncReturnsJsonArrayIfArray()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            dynamic result = fb.Get(_parameters);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.IsAssignableFrom<IList<object>>(result);
            Assert.IsType<JsonArray>(result);
        }

        [Fact]
        public void Async_CallsGetCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) => ++called;
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(1, called);
        }

        [Fact]
        public void Async_GetCompletedErrorIsNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            Exception error = null;

            TestExtensions.Do(evt => fb.GetCompleted += (o, e) => error = e.Error,
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Null(error);
        }

        [Fact]
        public void Async_GetCompletedGetResultDataIsNotNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            object result = null;

            TestExtensions.Do(evt => fb.GetCompleted += (o, e) => result = e.GetResultData(),
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.NotNull(result);
        }

        [Fact]
        public void Async_GetCompletedGetResultDataIsOfTypeJsonArrayIfArray()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            object result = null;

            TestExtensions.Do(evt => fb.GetCompleted += (o, e) => result = e.GetResultData(),
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.IsAssignableFrom<IList<object>>(result);
            Assert.IsType<JsonArray>(result);
        }

        [Fact]
        public void AsyncWithoutUserState_GetCompletedUserStateIsNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            object userState = null;

            TestExtensions.Do(evt => fb.GetCompleted += (o, e) => userState = e.UserState,
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Null(userState);
        }

        [Fact]
        public void Async_DoesNotCallPostCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            TestExtensions.Do(evt =>
                                  {
                                      fb.PostCompleted += (o, e) => ++called;
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void Async_DoesNotCallDeleteCompletedEvent()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;

            mockFb.ReturnsJson("[{\"uid\":\"4\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"name\":\"Mark Zuckerberg\"}]",
                 out mockRequest, out mockResponse);

            var fb = mockFb.Object;
            int called = 0;

            TestExtensions.Do(evt =>
                                  {
                                      fb.DeleteCompleted += (o, e) => ++called;
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_GetCompletedIsCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 called++;
                                                                 evt.Set();
                                                             };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.Equal(1, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_GetCompletedErrorIsNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            Exception error = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 error = e.Error;
                                                                 evt.Set();
                                                             };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.NotNull(error);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_GetCompletedErrorIsWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            Exception error = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 error = e.Error;
                                                                 evt.Set();
                                                             };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.IsAssignableFrom<WebExceptionWrapper>(error);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_GetCompletedGetResultDataIsNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            object result = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 result = e.GetResultData();
                                                                 evt.Set();
                                                             };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.Null(result);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_PostCompletedIsNotCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.PostCompleted += (o, e) =>
                                                              {
                                                                  called++;
                                                                  evt.Set();
                                                              };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.Equal(0, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsNotOpen_DeleteCompletedIsNotCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.NoInternetConnection(out mockRequest, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.DeleteCompleted += (o, e) =>
                                                                {
                                                                    called++;
                                                                    evt.Set();
                                                                };
                                  },
                              () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();

            Assert.Equal(0, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_GetCompletedIsCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 ++called;
                                                                 evt.Set();
                                                             };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(1, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_GetCompletedErrorIsNotNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            Exception error = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 error = e.Error;
                                                                 evt.Set();
                                                             };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.NotNull(error);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_GetCompletedErrorIsTypeOfWebExceptionWrapper()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            Exception error = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 error = e.Error;
                                                                 evt.Set();
                                                             };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.IsAssignableFrom<WebExceptionWrapper>(error);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_GetCompletedGetResultDataIsNull()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            object result = null;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.GetCompleted += (o, e) =>
                                                             {
                                                                 result = e.GetResultData();
                                                                 evt.Set();
                                                             };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Null(result);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_PostCompletedIsNotCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.PostCompleted += (o, e) =>
                                                              {
                                                                  ++called;
                                                                  evt.Set();
                                                              };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }

        [Fact]
        public void AsyncWhenThereIsNoInternetConnectionAndFiddlerIsOpen_DeleteCompletedIsNotCalled()
        {
            var mockFb = new Mock<FacebookClient> { CallBase = true };
            Mock<HttpWebRequestWrapper> mockRequest;
            Mock<HttpWebResponseWrapper> mockResponse;
            Mock<WebExceptionWrapper> mockWebException;

            mockFb.FiddlerNoInternetConnection(out mockRequest, out mockResponse, out mockWebException);

            int called = 0;
            var fb = mockFb.Object;

            TestExtensions.Do(evt =>
                                  {
                                      fb.DeleteCompleted += (o, e) =>
                                                                {
                                                                    ++called;
                                                                    evt.Set();
                                                                };
                                  }
                              , () => fb.GetAsync(_parameters), 5000);

            mockFb.VerifyCreateHttpWebRequest(Times.Once());
            mockRequest.VerifyBeginGetResponse();
            mockRequest.VerifyEndGetResponse();
            mockWebException.VerifyGetReponse();
            mockResponse.VerifyGetResponseStream();

            Assert.Equal(0, called);
        }
    }
}