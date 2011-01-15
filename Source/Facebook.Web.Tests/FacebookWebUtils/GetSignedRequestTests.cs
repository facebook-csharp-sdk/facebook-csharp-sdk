namespace Facebook.Web.Tests.FacebookWebUtils
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;
    using Xunit.Extensions;
    using System;

    public class GetSignedRequestTests
    {
        [Fact(DisplayName = "GetSignedRequest: Given no signed_request HttpRequest Param Then it should return null")]
        public void GetSignedRequest_GivenNoSignedRequestHttpRequestParam_ThenItShouldReturnNull()
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection());

            var signedRequest = FacebookWebUtils.GetSignedRequest("dummy_app_secret", requestMock.Object);

            Assert.Null(signedRequest);
        }

        [InlineData("543690fae0cd186965412ac4a49548b5", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "GetSignedRequest: Given a valid signed_request HttpRequest Param and secret Then the result should not be null")]
        public void GetSignedRequest_GivenAValidSignedRequestHttpRequestParamAndSecret_ThenTheResultShouldNotBeNull(string secret, string signedRequestValue)
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection { { "signed_request", signedRequestValue } });

            var signedRequest = FacebookWebUtils.GetSignedRequest(secret, requestMock.Object);

            Assert.NotNull(signedRequest);
        }

        [InlineData("Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0")]
        [Theory(DisplayName = "GetSignedRequest: Given a valid signed_request HttpRequest Param and invalid secret Then it should throw invalid operation exception")]
        public void GetSignedRequest_GivenAValidSignedRequestHttpRequestParamAndInvalidSecret_ThenItShouldThrowInvalidOperationException(string signedRequestValue)
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection { { "signed_request", signedRequestValue } });

            Assert.Throws<InvalidOperationException>(
                () => FacebookWebUtils.GetSignedRequest("wrong_secret", requestMock.Object));
        }
    }
}