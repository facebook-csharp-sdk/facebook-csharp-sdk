namespace Facebook.Web.Tests.CanvasUrlBuilder.PrepareCanvasLoginUrlOAuthState.GivenNullReturnUrlPath.GivenNullCancelUrlPath.GivenNullState.GivenNullLoginParameters
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenHttp_Beta_NoDefaultCancelUrlPathThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        private string _returnUrlPath;
        private string _cancelUrlPath;
        private string _state;
        private IDictionary<string, object> _loginParameters;

        public GivenHttp_Beta_NoDefaultCancelUrlPathThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                new DefaultFacebookApplication
                {
                    CanvasUrl = "http://localhost:16151/CSASPNETFacebookApp/",
                    CanvasPage = "http://apps.facebook.com/csharpsamplestwo/"
                },
                GetHttpRequest());
        }

        [Fact]
        public void ResultIsOfTypeJsonObject()
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, _cancelUrlPath, _state, _loginParameters);

            Assert.IsType<JsonObject>(result);
        }

        [Fact]
        public void ResultContainsR()
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, _cancelUrlPath, _state, _loginParameters);

            Assert.True(result.ContainsKey("r"));
        }

        [Fact]
        public void ResultDoesNotContainC()
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, _cancelUrlPath, _state, _loginParameters);

            Assert.False(result.ContainsKey("c"));
        }

        [Fact]
        public void RIsSetCorrectly()
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, _cancelUrlPath, _state, _loginParameters);

            Assert.Equal("http://apps.beta.facebook.com/csharpsamplestwo/default.aspx", result["r"]);
        }

        [Fact]
        public void ResultDoesNotContainS()
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, _cancelUrlPath, _state, _loginParameters);

            Assert.False(result.ContainsKey("s"));
        }

        [Fact]
        public void IsSecuredConnectionIsFalse()
        {
            Assert.False(_canvasUrlBuilder.IsSecureConnection);
        }

        [Fact]
        public void UseFacebookBetaIsTrue()
        {
            Assert.True(_canvasUrlBuilder.UseFacebookBeta);
        }

        public HttpRequestBase GetHttpRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.Url).Returns(new Uri("http://localhost:16151/CSASPNETFacebookApp/default.aspx"));
            requestMock.Setup(request => request.ApplicationPath).Returns("/CSASPNETFacebookApp");
            requestMock.Setup(request => request.RawUrl).Returns("/CSASPNETFacebookApp/");
            requestMock.Setup(request => request.UrlReferrer).Returns(new Uri("http://apps.beta.facebook.com/csharpsamplestwo/"));

            return requestMock.Object;
        }
    }
}