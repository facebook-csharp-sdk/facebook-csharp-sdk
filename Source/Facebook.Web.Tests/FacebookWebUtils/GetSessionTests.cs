namespace Facebook.Web.Tests.FacebookWebUtils
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class GetSessionTests
    {
        [Fact(DisplayName = "GetSession: Given a http request with no signed request or fb session cookies Then it should return null")]
        public void GetSession_GivenAHttpRequestWithNoSignedRequestOrFbSessionCookies_ThenItShouldReturnNull()
        {
            var requestParams = new NameValueCollection();
            var request = GetHttpRequest(requestParams);
            var appId = "dummy";
            var appSecret = "dummy";

            var session = FacebookWebUtils.GetSession(appId, appSecret, request);

            Assert.Null(session);
        }

        [InlineData("543690fae0cd186965412ac4a49548b5", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "GetSession: Given a http request with a valid signed request and valid app secret Then it should not return null")]
        public void GetSession_GivenAHttpRequestWithAValidSignedRequestAndValidAppSecret_ThenItShouldNotReturnNull(string appSecret, string signedRequestValue)
        {
            var requestParams = new NameValueCollection { { "signed_request", signedRequestValue } };
            var request = GetHttpRequest(requestParams);
            var appId = "dummy";

            var session = FacebookWebUtils.GetSession(appId, appSecret, request);

            Assert.NotNull(session);
        }

        [InlineData("Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "GetSession: Given a http request with a valid signed request and invalid app secret Then it should throw InvalidOperationException")]
        public void GetSession_GivenAHttpRequestWithAValidSignedRequestAndInvalidAppSecret_ThenItShouldThrowInvalidOperationException(string signedRequestValue)
        {
            var requestParams = new NameValueCollection { { "signed_request", signedRequestValue } };
            var request = GetHttpRequest(requestParams);
            var appId = "dummy";
            var appSecret = "invalid_secret";

            Assert.Throws<InvalidOperationException>(() => FacebookWebUtils.GetSession(appId, appSecret, request));
        }

        private static HttpRequestBase GetHttpRequest(NameValueCollection requestParams)
        {
            Contract.Requires(requestParams != null);

            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(request => request.Params).Returns(requestParams);

            return requestMock.Object;
        }
    }
}