namespace Facebook.Web.Tests.FacebookSignedRequest.TryParse.internal_method
{
    using System;
    using Facebook;
    using Xunit;

    public class GivenASingedRequestvalueWithoutSignatureThen
    {
        [Fact]
        public void ShouldThrowInvalidOperationException()
        {
            var signedRequest = ".envelope_only";
            string secret = "secret";
            int maxAge = 3600;
            double currentTime = 1297678642.8070507;

            Assert.Throws<InvalidOperationException>(() => FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true));
        }

        [Fact]
        public void ErrorMessageShouldBeInvalidSingedRequest()
        {
            var signedRequest = ".envelope_only";
            string secret = "secret";
            int maxAge = 3600;
            double currentTime = 1297678642.8070507;

            Exception exception = null;
            try
            {
                FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }

            Assert.Equal(Properties.Resources.InvalidSignedRequest, exception.Message);
        }
    }
}