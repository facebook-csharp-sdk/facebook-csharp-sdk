namespace Facebook.Web.Tests.FacebookWebUtils.VerifyPostSubscription
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenAValidPostSubscriptionRequestThen
    {
        [Fact]
        public void ReturnsTrue()
        {
            var request = GetRequest();
            string errorMessage;
            string secret = "18b8c40f4e48e2616a0c548ec96fdeb2";
            string jsonString = "{\"object\":\"user\",\"entry\":[{\"uid\":\"100000326364373\",\"id\":\"100000326364373\",\"time\":1297087737,\"changed_fields\":[\"feed\"]}]}";

            var result = FacebookSubscriptionVerifier.VerifyPostSubscription(request, secret, jsonString, out errorMessage);

            Assert.True(result);
        }

        [Fact]
        public void ErrorMessageIsNull()
        {
            var request = GetRequest();
            string errorMessage;
            string secret = "18b8c40f4e48e2616a0c548ec96fdeb2";
            string jsonString = "{\"object\":\"user\",\"entry\":[{\"uid\":\"100000326364373\",\"id\":\"100000326364373\",\"time\":1297087737,\"changed_fields\":[\"feed\"]}]}";

            var result = FacebookSubscriptionVerifier.VerifyPostSubscription(request, secret, jsonString, out errorMessage);

            Assert.Null(errorMessage);
        }

        private static HttpRequestBase GetRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.HttpMethod).Returns("POST");
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection { { "HTTP_X_HUB_SIGNATURE", "sha1=e2b4478917846833df7e3b2c6ad71adc5e6fa1b8" } });

            return requestMock.Object;
        }
    }
}