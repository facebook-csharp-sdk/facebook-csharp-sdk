namespace Facebook.Web.Tests.FacebookSignedRequest.TryParse.internal_method
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenASignedRequestWithMoreThanOneDotThen
    {
        [Theory]
        [PropertyData("SignedRequestWithMoreThanOneDot")]
        public void ShouldThrowInvalidOperationException(string signedRequestWithMoreThanTwoDots)
        {
            string secret = "secret";
            int maxAge = 3600;
            double currentTime = 1297678642.8070507;

            Assert.Throws<InvalidOperationException>(() => FacebookSignedRequest.TryParse(secret, signedRequestWithMoreThanTwoDots, maxAge, currentTime, true));
        }

        [Theory]
        [PropertyData("SignedRequestWithMoreThanOneDot")]
        public void ErrorMessageShouldBeInvalidSingedRequest(string signedRequestWithMoreThanTwoDots)
        {
            string secret = "secret";
            int maxAge = 3600;
            double currentTime = 1297678642.8070507;

            Exception exception = null;
            try
            {
                FacebookSignedRequest.TryParse(secret, signedRequestWithMoreThanTwoDots, maxAge, currentTime, true);
            }
            catch (InvalidOperationException ex)
            {
                exception = ex;
            }

            Assert.Equal(Properties.Resources.InvalidSignedRequest, exception.Message);
        }

        public static IEnumerable<object[]> SignedRequestWithMoreThanOneDot
        {
            get
            {
                yield return new object[] { "two.dot.s" };
                yield return new object[] { "th.ree.dot.s" };
                yield return new object[] { "f.ou.r.dot.s" };
            }
        }
    }
}