namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenAUrlReferrer
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class AndHttpsUrlAndReferUrlAsNonAppBetaFacebookThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public AndHttpsUrlAndReferUrlAsNonAppBetaFacebookThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("https://localhost:16151/CSASPNETFacebookApp/default.aspx"),
                new Uri("https://apps.facebook.com/app/default.aspx")));
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