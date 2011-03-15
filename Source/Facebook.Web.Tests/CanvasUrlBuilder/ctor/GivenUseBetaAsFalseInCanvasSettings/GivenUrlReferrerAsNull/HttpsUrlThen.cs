namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenUrlReferrerAsNull
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class HttpsUrlThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public HttpsUrlThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("https://localhost:16151/CSASPNETFacebookApp/default.aspx"), null));
        }

        [Fact]
        public void IsSecuredConnectionIsTrue()
        {
            Assert.True(_canvasUrlBuilder.IsSecureConnection);
        }

        [Fact]
        public void UseFacebookBetaIsFalse()
        {
            Assert.False(_canvasUrlBuilder.UseFacebookBeta);
        }
    }
}
