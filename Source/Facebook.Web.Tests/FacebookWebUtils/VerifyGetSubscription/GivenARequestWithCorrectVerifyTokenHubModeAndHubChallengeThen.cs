namespace Facebook.Web.Tests.FacebookWebUtils.VerifyGetSubscription
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenARequestWithCorrectVerifyTokenHubModeAndHubChallengeThen
    {
        [Fact]
        public void ReturnsTrue()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookSubscriptionVerifier.VerifyGetSubscription(request, "correct_verify_token", out errorMessage);

            Assert.True(result);
        }

        [Fact]
        public void ErrorMessageIsNull()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookSubscriptionVerifier.VerifyGetSubscription(request, "correct_verify_token", out errorMessage);

            Assert.Null(errorMessage);
        }

        private static HttpRequestBase GetRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.HttpMethod).Returns("GET");
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection
                                                                     {
                                                                         { "hub.verify_token", "correct_verify_token" },
                                                                         { "hub.mode","subscribe" },
                                                                         { "hub.challenge","dummy_challenge" }
                                                                     });

            return requestMock.Object;
        }
    }
}