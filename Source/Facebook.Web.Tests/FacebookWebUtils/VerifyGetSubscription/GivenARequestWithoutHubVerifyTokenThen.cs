namespace Facebook.Web.Tests.FacebookWebUtils.VerifyGetSubscription
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenARequestWithoutHubVerifyTokenAndWithHubModeAsSubscribeThen
    {
        [Fact]
        public void ReturnsFalse()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookSubscriptionVerifier.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.False(result);
        }

        [Fact]
        public void ErrorMessageIsNotNull()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookSubscriptionVerifier.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void ErrorMessageIsSetCorrectly()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookSubscriptionVerifier.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.Equal(Properties.Resources.InvalidVerifyToken, errorMessage);
        }

        private static HttpRequestBase GetRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.HttpMethod).Returns("GET");
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection { { "hub.mode", "subscribe" } });

            return requestMock.Object;
        }
    }
}