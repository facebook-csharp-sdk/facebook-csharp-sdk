namespace Facebook.Web.Tests
{
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Moq;
    using Xunit;

    public class FacebookWebUtilsTests
    {
        [Fact(DisplayName = "GetSessionCookieValue: Given a http request with no fb session cookie Then it should return null")]
        public void GetSessionCookieValue_GivenAHttpRequestWithNoFbSessionCookie_ThenItShouldReturnNull()
        {
            var requestParams = new NameValueCollection();
            var request = GetHttpRequest(requestParams);
            var appId = "dummy";

            var cookieValue = Web.FacebookWebUtils.GetSessionCookieValue(appId, request);

            Assert.Null(cookieValue);
        }

        [Fact(DisplayName = "GetSessionCookieValue: Given a http request with a valid fb session cookie Then it should not return null")]
        public void GetSessionCookieValue_GivenAHttpRequestWithAValidFbSessionCookie_ThenItShouldNotReturnNull()
        {
            var requestParams = new NameValueCollection
                                    {
                                        { "fbs_dummyappid", "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026" }
                                    };

            var request = GetHttpRequest(requestParams);
            var appId = "dummyappid";

            var cookieValue = Web.FacebookWebUtils.GetSessionCookieValue(appId, request);

            Assert.NotNull(cookieValue);
        }

        private static HttpRequestBase GetHttpRequest(NameValueCollection requestParams)
        {
            Contract.Requires(requestParams != null);

            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(request => request.Params).Returns(requestParams);

            return requestMock.Object;
        }
    }
}