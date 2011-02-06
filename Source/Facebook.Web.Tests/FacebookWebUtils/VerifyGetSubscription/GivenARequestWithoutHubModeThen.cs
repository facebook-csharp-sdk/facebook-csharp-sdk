namespace Facebook.Web.Tests.FacebookWebUtils.VerifyGetSubscription
{
    using System.Collections.Specialized;
    using System.Web;
    using Facebook.Web;
    using Moq;
    using Xunit;

    public class GivenARequestWithoutHubModeThen
    {
        [Fact]
        public void ReturnsFalse()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookWebUtils.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.False(result);
        }

        [Fact]
        public void ErrorMessageIsNotNull()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookWebUtils.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void ErrorMessageIsSetCorrectly()
        {
            var request = GetRequest();
            string errorMessage;

            var result = FacebookWebUtils.VerifyGetSubscription(request, "dummy verify token", out errorMessage);

            Assert.Equal(FacebookWebUtils.ERRORMSG_SUBSCRIPTION_HUBMODE, errorMessage);
        }

        private static HttpRequestBase GetRequest()
        {
            var requestMock = new Mock<HttpRequestBase>();

            requestMock.Setup(request => request.HttpMethod).Returns("GET");
            requestMock.Setup(request => request.Params).Returns(new NameValueCollection());

            return requestMock.Object;
        }
    }
}