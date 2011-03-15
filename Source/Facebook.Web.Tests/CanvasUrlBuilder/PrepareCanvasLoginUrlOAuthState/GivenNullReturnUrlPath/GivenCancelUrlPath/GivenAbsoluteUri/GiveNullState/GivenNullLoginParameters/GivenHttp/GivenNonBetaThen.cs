namespace Facebook.Web.Tests.CanvasUrlBuilder.PrepareCanvasLoginUrlOAuthState.GivenNullReturnUrlPath.GivenCancelUrlPath.GivenAbsoluteUri.GiveNullState.GivenNullLoginParameters.GivenHttp
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class GivenNonBetaThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        private string _returnUrlPath;
        private string _state;
        private IDictionary<string, object> _loginParameters;

        public GivenNonBetaThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                new DefaultFacebookApplication
                {
                    CanvasUrl = "http://localhost:16151/CSASPNETFacebookApp/",
                    CanvasPage = "http://apps.facebook.com/csharpsamplestwo/"
                },
                GetHttpRequest());
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultIsOfTypeJsonObject(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.IsType<JsonObject>(result);
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultContainsR(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.True(result.ContainsKey("r"));
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultContainsC(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.True(result.ContainsKey("c"));
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void RIsSetCorrectly(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.Equal("http://apps.facebook.com/csharpsamplestwo/default.aspx", result["r"]);
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void CIsSetCorrectly(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.Equal(cancelUrlPath, result["c"]);
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultDoesNotContainS(string cancelUrlPath)
        {
            var result = _canvasUrlBuilder.PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, cancelUrlPath, _state, _loginParameters);

            Assert.False(result.ContainsKey("s"));
        }

        [Fact]
        public void IsSecuredConnectionIsFalse()
        {
            Assert.False(_canvasUrlBuilder.IsSecureConnection);
        }

        [Fact]
        public void UseFacebookBetaIsFalse()
        {
            Assert.False(_canvasUrlBuilder.UseFacebookBeta);
        }

        public static IEnumerable<object[]> CancelUrlPath
        {
            get { return CanvasUrlBuilderHelper.CancelUrlPathAbsoluteUri; }
        }

        public HttpRequestBase GetHttpRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.Url).Returns(new Uri("http://localhost:16151/CSASPNETFacebookApp/default.aspx"));
            requestMock.Setup(request => request.ApplicationPath).Returns("/CSASPNETFacebookApp");
            requestMock.Setup(request => request.RawUrl).Returns("/CSASPNETFacebookApp/");
            requestMock.Setup(request => request.UrlReferrer).Returns(new Uri("http://apps.facebook.com/csharpsamplestwo/"));

            return requestMock.Object;
        }
    }
}