namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenUrlReferrerAsNull
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class HttpUrlThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public HttpUrlThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("http://localhost:16151/CSASPNETFacebookApp/default.aspx"), null));
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
    }
}
