namespace Facebook.Web.Tests.Authorizer
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Facebook.Web.New;
    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class SignedRequestTests
    {
        [Fact(DisplayName = "SignedRequest: When there is no signed_request HttpRequest Param Then it should return null")]
        public void SignedRequest_WhenThereIsNoSigned_requestHttpRequestParam_ThenItShouldReturnNull()
        {
            var requestParams = new NameValueCollection();
            var context = GetHttpContext(requestParams);
            var settings = new FacebookSettings { AppId = "dummy", AppSecret = "dummy" };
            var authorizer = new Authorizer(settings, context);

            var signedRequest = authorizer.SignedRequest;

            Assert.Null(signedRequest);
        }

        [InlineData("543690fae0cd186965412ac4a49548b5", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "SignedRequest: When there is a valid signed_request HttpRequest Param and app secret Then result should not be null")]
        public void SignedRequest_WhenThereIsAValidSigned_requestHttpRequestParamAndAppSecret_ThenResultShouldNotBeNull(string secret, string signedRequestValue)
        {
            var requestParams = new NameValueCollection { { "signed_request", signedRequestValue } };
            var context = GetHttpContext(requestParams);
            var settings = new FacebookSettings { AppId = "dummy", AppSecret = secret };
            var authorizer = new Authorizer(settings, context);

            var signedRequest = authorizer.SignedRequest;

            Assert.NotNull(signedRequest);
        }

        [InlineData("Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "SignedRequest: When there is a valid signed_request HttpRequest Param and invalid app secret Then it should throw InvalidOperationException")]
        public void SignedRequest_WhenThereIsAValidSigned_requestHttpRequestParamAndInvalidAppSecret_ThenItShouldThrowInvalidOperationException(string signedRequestValue)
        {
            var requestParams = new NameValueCollection { { "signed_request", signedRequestValue } };
            var context = GetHttpContext(requestParams);
            var settings = new FacebookSettings { AppId = "dummy", AppSecret = "wrong_secret" };
            var authorizer = new Authorizer(settings, context);

            Assert.Throws<InvalidOperationException>(() => authorizer.SignedRequest);
        }

        private static HttpContextBase GetHttpContext(NameValueCollection requestParams)
        {
            Contract.Requires(requestParams != null);

            var contextMock = new Mock<HttpContextBase>();
            var responseMock = new Mock<HttpResponseBase>();
            var requestMock = new Mock<HttpRequestBase>();

            contextMock.Setup(context => context.Request).Returns(requestMock.Object);
            contextMock.Setup(context => context.Response).Returns(responseMock.Object);

            requestMock.Setup(request => request.Params).Returns(requestParams);

            return contextMock.Object;
        }
    }
}