namespace Facebook.Web.Tests.CanvasUrlBuilder.ctor.GivenUseBetaAsFalseInCanvasSettings.GivenAUrlReferrer
{
    using System;
    using Facebook.Web;
    using Xunit;

    public class AndHttpUrlAndReferUrlAsAppBetaFacebookThen
    {
        private CanvasUrlBuilder _canvasUrlBuilder;

        public AndHttpUrlAndReferUrlAsAppBetaFacebookThen()
        {
            _canvasUrlBuilder = new CanvasUrlBuilder(
                CanvasUrlBuilderHelper.GetFakeFacebookApplication(false),
                CanvasUrlBuilderHelper.GetFakeHttpRequest(
                new Uri("http://localhost:16151/CSASPNETFacebookApp/default.aspx"),
                new Uri("http://apps.beta.facebook.com/app/default.aspx")));
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
    }
}