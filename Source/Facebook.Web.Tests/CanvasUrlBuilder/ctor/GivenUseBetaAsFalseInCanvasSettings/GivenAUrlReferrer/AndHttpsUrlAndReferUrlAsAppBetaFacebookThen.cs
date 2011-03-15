namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenAUrlReferrer
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class AndHttpsUrlAndReferUrlAsAppBetaFacebookThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public AndHttpsUrlAndReferUrlAsAppBetaFacebookThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("https://localhost:16151/CSASPNETFacebookApp/default.aspx"),
                new Uri("https://apps.beta.facebook.com/app/default.aspx")));
        }

        [Fact]
        public void IsSecuredConnectionIsTrue()
        {
            Assert.True(_canvasUrlBuilder.IsSecureConnection);
        }

        [Fact]
        public void UseFacebookBetaIsTrue()
        {
            Assert.True(_canvasUrlBuilder.UseFacebookBeta);
        }
    }
}