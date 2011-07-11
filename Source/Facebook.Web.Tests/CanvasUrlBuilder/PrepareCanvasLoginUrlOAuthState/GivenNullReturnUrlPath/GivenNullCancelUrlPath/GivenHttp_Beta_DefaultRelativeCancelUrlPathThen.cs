namespace Facebook.Web.Tests.CanvasUrlBuilder.PrepareCanvasLoginUrlOAuthState.GivenNullReturnUrlPath.GivenNullCancelUrlPath.GivenNullState.GivenNullLoginParameters
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class GivenHttp_Beta_DefaultRelativeCancelUrlPathThen
    {
        private string _returnUrlPath;
        private string _state;
        private IDictionary<string, object> _loginParameters;

        public CanvasUrlBuilder GetCanvasUrlBuilder(string cancelUrlPath)
        {
            return new CanvasUrlBuilder(
                new DefaultFacebookApplication
                    {
                        CanvasUrl = "http://localhost:16151/CSASPNETFacebookApp/",
                        CanvasPage = "http://apps.facebook.com/csharpsamplestwo/",
                        CancelUrlPath = cancelUrlPath
                    },
                GetHttpRequest());
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultIsOfTypeJsonObject(string cancelUrlPath)
        {

            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, null, _state, _loginParameters);

            Assert.IsType<JsonObject>(result);
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultContainsR(string cancelUrlPath)
        {
            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
                _returnUrlPath, null, _state, _loginParameters);

            Assert.True(result.ContainsKey("r"));
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultContainsC(string cancelUrlPath)
        {
            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, null, _state, _loginParameters);

            Assert.True(result.ContainsKey("c"));
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void RIsSetCorrectly(string cancelUrlPath)
        {
            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, null, _state, _loginParameters);

            Assert.Equal("http://apps.beta.facebook.com/csharpsamplestwo/default.aspx", result["r"]);
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void ResultDoesNotContainS(string cancelUrlPath)
        {
            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
             _returnUrlPath, null, _state, _loginParameters);

            Assert.False(result.ContainsKey("s"));
        }

        [Theory]
        [PropertyData("CancelUrlPath")]
        public void CIsSetCorrectly(string cancelUrlPath)
        {
            var result = GetCanvasUrlBuilder(cancelUrlPath).PrepareCanvasLoginUrlOAuthState(
               _returnUrlPath, null, _state, _loginParameters);

            cancelUrlPath = FacebookWebUtils.RemoveStartingSlash(cancelUrlPath);

            Assert.Equal("http://apps.beta.facebook.com/csharpsamplestwo/" + cancelUrlPath, result["c"]);
        }

        [Fact]
        public void IsSecuredConnectionIsFalse()
        {
            Assert.False(GetCanvasUrlBuilder(null).IsSecureConnection);
        }

        [Fact]
        public void UseFacebookBetaIsTrue()
        {
            Assert.True(GetCanvasUrlBuilder(null).UseFacebookBeta);
        }

        public static IEnumerable<object[]> CancelUrlPath
        {
            get { return CanvasUrlBuilderHelper.CancelUrlPathRelativeUri; }
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