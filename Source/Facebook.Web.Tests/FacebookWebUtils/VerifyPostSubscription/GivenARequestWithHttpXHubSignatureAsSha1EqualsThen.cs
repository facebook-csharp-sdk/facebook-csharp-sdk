namespace Facebook.Web.Tests.FacebookWebUtils.VerifyPostSubscription
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenARequestWithHttpXHubSignatureAsSha1EqualsThen
    {
        [Fact]
        public void ReturnsFalse()
        {
            var request = GetRequest();
            string errorMessage;
            string dummyJson = "{}";

            var result = FacebookSubscriptionVerifier.VerifyPostSubscription(request, "dummy_secret", dummyJson, out errorMessage);

            Assert.False(result);
        }

        [Fact]
        public void ErrorMessageIsNotNull()
        {
            var request = GetRequest();
            string errorMessage;
            string dummyJson = "{}";

            var result = FacebookSubscriptionVerifier.VerifyPostSubscription(request, "dummy_secret", dummyJson, out errorMessage);

            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void ErrorMessageIsSetCorrectly()
        {
            var request = GetRequest();
            string errorMessage;
            string dummyJson = "{}";

            var result = FacebookSubscriptionVerifier.VerifyPostSubscription(request, "dummy_secret", dummyJson, out errorMessage);

            Assert.Equal(Properties.Resources.InvalidHttpXHubSignature, errorMessage);
        }

        private static HttpRequestBase GetRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.HttpMethod).Returns("POST");
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection { { "HTTP_X_HUB_SIGNATURE", "sha1=" } });

            return requestMock.Object;
        }
    }
}