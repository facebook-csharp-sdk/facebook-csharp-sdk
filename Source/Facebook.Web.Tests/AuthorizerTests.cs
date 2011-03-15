
namespace Facebook.Web.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Web;
    using Moq;
    using Xunit;

    public class AuthorizerTests
    {
        [Fact(DisplayName = "Session: Given a request with no signed request and fb session cookie Then it should return null")]
        public void Session_GivenARequestWithNoSignedRequestAndFbSessionCookie_ThenItShouldReturnNull()
        {
            var requestParams = new NameValueCollection();
            var httpContext = GetHttpContext(requestParams);

            var authorizer =
                new FacebookWebAuthorizer(new DefaultFacebookApplication { AppId = "dummy", AppSecret = "dummy" },
                                          httpContext);
            var session = authorizer.FacebookWebRequest.Session;

            Assert.Null(session);
        }

        [Fact(DisplayName = "Session: Given a request with valid signed request and no fb session cookie Then it should not return null")]
        public void Session_GivenARequestWithValidSignedRequestAndNoFbSessionCookie_ThenItShouldNotReturnNull()
        {
            var requestParams = new NameValueCollection
                                    {
                                        { "signed_request", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0" }
                                    };

            var httpContext = GetHttpContext(requestParams);

            var authorizer =
                new FacebookWebAuthorizer(new DefaultFacebookApplication { AppId = "dummy", AppSecret = "543690fae0cd186965412ac4a49548b5" },
                                          httpContext);

            var session = authorizer.FacebookWebRequest.Session;

            Assert.NotNull(session);
        }

        [Fact(DisplayName = "Session: Given a request with valid signed request and no fb session cookie and invalid appsecret Then it should throw InvalidOperationException")]
        public void Session_GivenARequestWithValidSignedRequestAndNoFbSessionCookieAndInvalidAppsecret_ThenItShouldThrowInvalidOperationException()
        {
            var requestParams = new NameValueCollection
                                    {
                                        { "signed_request", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0" }
                                    };

            var httpContext = GetHttpContext(requestParams);
            var authorizer = new FacebookWebAuthorizer(
                new DefaultFacebookApplication { AppId = "dummy", AppSecret = "invalid_secret" }, httpContext);

            Assert.Throws<InvalidOperationException>(() => authorizer.FacebookWebRequest.Session);
        }

        [Fact(DisplayName = "Session: Given a request with valid session cookie and no signed request and valid secret Then it should not return null")]
        public void Session_GivenARequestWithValidSessionCookieAndNoSignedRequestAndValidSecret_ThenItShouldNotReturnNull()
        {
            var appId = "124973200873702";
            var appSecret = "3b4a872617be2ae1932baa1d4d240272";
            var requestParams = new NameValueCollection { { "fbs_" + appId, "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026" } };

            var httpContext = GetHttpContext(requestParams);

            var authorizer = new FacebookWebAuthorizer(
                new DefaultFacebookApplication { AppId = appId, AppSecret = appSecret }, httpContext);

            var session = authorizer.FacebookWebRequest.Session;

            Assert.NotNull(session);
        }

        [Fact(DisplayName = "Session: Given a request with valid session cookie, signed request, app id and secret Then it should not return null")]
        public void Session_GivenARequestWithValidSessionCookieSignedRequestAppIdAndSecret_ThenItShouldNotReturnNull()
        {
            var appId = "dummy";
            var appSecret = "543690fae0cd186965412ac4a49548b5";
            var requestParams = new NameValueCollection
                                    {
                                        { "signed_request", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0" },
                                        { "fbs_" + appId, "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026" }
                                    };
            var httpContext = GetHttpContext(requestParams);

            var authorizer = new FacebookWebAuthorizer(
                new DefaultFacebookApplication { AppId = appId, AppSecret = appSecret }, httpContext);

            var session = authorizer.FacebookWebRequest.Session;

            Assert.NotNull(session);
        }

        [Fact(DisplayName = "Session: Given a request with valid session cookie, signed request, app id and secret Then the access token should be equal to that of signed request")]
        public void Session_GivenARequestWithValidSessionCookieSignedRequestAppIdAndSecret_ThenTheAccessTokenShouldBeEqualToThatOfSignedRequest()
        {
            var appId = "dummy";
            var appSecret = "543690fae0cd186965412ac4a49548b5";
            var requestParams = new NameValueCollection
                                    {
                                        { "signed_request", "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0" },
                                        { "fbs_" + appId, "access_token=124973200873702%7C2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026%7Cvz4H9xjlRZPfg2quCv0XOM5g9_o&expires=1295118000&secret=lddpssZCuPoEtjcDFcWtoA__&session_key=2.OAaqICOCk_B4sZNv59q8Yg__.3600.1295118000-100001327642026&sig=1d95fa4b3dfa5b26c01c8ac8676d80b8&uid=100001327642026" }
                                    };
            var httpContext = GetHttpContext(requestParams);

            var authorizer = new FacebookWebAuthorizer(
                new DefaultFacebookApplication { AppId = appId, AppSecret = appSecret }, httpContext);

            var session = authorizer.FacebookWebRequest.Session;
            var accessToken = session.AccessToken;

            Assert.Equal("120625701301347|2.I3WPFn_9kJegQNDf5K_I2g__.3600.1282928400-14812017|qrfiOepbv4fswcdYtRWfANor9bQ.", accessToken);
        }

        private static HttpContextBase GetHttpContext(NameValueCollection requestParams)
        {
            Contract.Requires(requestParams != null);

            var httpContextMock = new Mock<HttpContextBase>();
            var requestMock = new Mock<HttpRequestBase>();
            var responseMock = new Mock<HttpResponseBase>();

            requestMock.Setup(request => request.Params).Returns(requestParams);
            requestMock.Setup(request => request.Url).Returns(new Uri("http://app.facebook.com/appname"));

            httpContextMock.Setup(context => context.Request).Returns(requestMock.Object);
            httpContextMock.Setup(context => context.Response).Returns(responseMock.Object);
            httpContextMock.Setup(context => context.Items).Returns(new System.Collections.Generic.Dictionary<object, object>());

            return httpContextMock.Object;
        }
    }
}