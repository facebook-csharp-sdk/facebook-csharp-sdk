namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenAUrlReferrer
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class AndHttpUrlAndReferUrlAsNonAppBetaFacebookThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public AndHttpUrlAndReferUrlAsNonAppBetaFacebookThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("http://localhost:16151/CSASPNETFacebookApp/default.aspx"),
                new Uri("http://apps.facebook.com/app/default.aspx")));
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