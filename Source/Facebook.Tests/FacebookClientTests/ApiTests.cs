namespace Facebook.Tests.FacebookClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using global::Facebook;
    using Xunit;

    public class ApiHttpWebRequestTests
    {
        [Fact]
        public void GetTest()
        {
            var fb = new FacebookClient();
            FakeHttpWebRequestWrapper fakeRequest = null;
            FakeHttpWebResponseWrapper fakeResponse = null;

            fb.HttpWebRequestFactory =
                uri => fakeRequest =
                       new FakeHttpWebRequestWrapper()
                           .WithRequestUri(uri)
                           .FakeResponse()
                           .WithResponseStreamAs(
                               "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                           .WithContentType("text/javascript; charset=UTF-8")
                           .WithStatusCode(200)
                           .GetFakeHttpWebRequestWrapper();

            dynamic result = fb.Get("4");

            Assert.Equal("GET", fakeRequest.Method);
            Assert.Equal("https://graph.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
            Assert.True(fakeRequest.ContentLength == 0);

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
            Assert.Equal("4", result.id);
            Assert.Equal("Mark Zuckerberg", result.name);
        }

        public class WhenAccessTokenPropertyIsSet
        {
            [Fact]
            public void ShouldContainAccessTokenInQuerystring()
            {
                var fb = new FacebookClient();
                FakeHttpWebRequestWrapper fakeRequest = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                fb.AccessToken = "dummy_access_token";

                dynamic result = fb.Get("me");

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/me?access_token=dummy_access_token", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }

            public class WhenParameterContainsAccessToken
            {
                [Fact]
                public void IfAccessTokenInParameterIsEmptyShouldNotContainAccessToken()
                {
                    var fb = new FacebookClient();
                    FakeHttpWebRequestWrapper fakeRequest = null;

                    fb.HttpWebRequestFactory =
                        uri => fakeRequest =
                               new FakeHttpWebRequestWrapper()
                                   .WithRequestUri(uri)
                                   .FakeResponse()
                                   .WithResponseStreamAs(
                                       "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                                   .WithContentType("text/javascript; charset=UTF-8")
                                   .WithStatusCode(200)
                                   .GetFakeHttpWebRequestWrapper();

                    fb.AccessToken = "dummy_access_token";

                    var parameters = new Dictionary<string, object>();
                    parameters["access_token"] = string.Empty;
                    dynamic result = fb.Get("4", parameters);

                    Assert.Equal("GET", fakeRequest.Method);
                    Assert.Equal("https://graph.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
                    Assert.True(fakeRequest.ContentLength == 0);

                    Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                    Assert.Equal("4", result.id);
                    Assert.Equal("Mark Zuckerberg", result.name);
                }
            }

            [Fact]
            public void IfAccessTokenInParameterIsNullShouldNotContainAccessToken()
            {
                var fb = new FacebookClient();
                FakeHttpWebRequestWrapper fakeRequest = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                fb.AccessToken = "dummy_access_token";

                var parameters = new Dictionary<string, object>();
                parameters["access_token"] = null;
                dynamic result = fb.Get("4", parameters);

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }
        }

        public class UseBeta
        {
            [Fact]
            public void UseBetaUrlWhenSetToTrue()
            {
                var fb = new FacebookClient { UseFacebookBeta = true };

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                dynamic result = fb.Get("4");

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.beta.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }
        }

        public class IsSecureConnection
        {
            [Fact]
            public void ContainsReturnSslResourcesAsTrueWhenSetToTrue()
            {
                var fb = new FacebookClient { IsSecureConnection = true };

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                dynamic result = fb.Get("4");

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/4?return_ssl_resources=true", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }

            [Fact]
            public void DoesNotContainReturnSslResourcesWhenSetToFalse()
            {
                var fb = new FacebookClient { IsSecureConnection = false };

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                dynamic result = fb.Get("4");

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }
        }

        public class WhenParameterContainsFormat
        {
            [Fact]
            public void OverrideFormatWithJsonString()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["format"] = "xml";
                dynamic result = fb.Get("4", parameters);

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/4?format=json-strings", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.Equal("4", result.id);
                Assert.Equal("Mark Zuckerberg", result.name);
            }
        }

        public class WhenParameterContainsMethodAsDelete
        {
            [Fact]
            public void ShouldThrowError()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["method"] = "delete";

                Assert.Throws<ArgumentException>(() => fb.Get("4", parameters));
            }

            [Fact]
            public void ShouldThrowErrorCaptialLetters()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["method"] = "DELETE";

                Assert.Throws<ArgumentException>(() => fb.Get("4", parameters));
            }
        }

        public class Attachments
        {
            [Fact]
            public void FacebookMediaObjectInGet()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["file"] = new FacebookMediaObject();

                Exception exception = null;
                try
                {
                    fb.Get("me/feed", parameters);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Attachments (FacebookMediaObject/FacebookMediaStream) are valid only in POST requests.", exception.Message);
            }

            [Fact]
            public void FacebookMediaStreamInGet()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["file"] = new FacebookMediaStream();

                Exception exception = null;
                try
                {
                    fb.Get("me/feed", parameters);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Attachments (FacebookMediaObject/FacebookMediaStream) are valid only in POST requests.", exception.Message);
            }

            [Fact]
            public void FacebookMediaObjectInDelete()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["file"] = new FacebookMediaObject();

                Exception exception = null;
                try
                {
                    fb.Delete("id", parameters);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Attachments (FacebookMediaObject/FacebookMediaStream) are valid only in POST requests.", exception.Message);
            }

            [Fact]
            public void FacebookMediaStreamInDelete()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .FakeResponse()
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["file"] = new FacebookMediaStream();

                Exception exception = null;
                try
                {
                    fb.Delete("id", parameters);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
                Assert.Equal("Attachments (FacebookMediaObject/FacebookMediaStream) are valid only in POST requests.", exception.Message);
            }
        }

        public class ETag
        {
            [Fact]
            public void NotModifedRespnse()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse()
                               .As304NotModified(
                                   res => fakeResponse = res
                                              .WithContentType("text/javascript; charset=UTF-8")
                                              .WithHeader("X-FB-Rev", "548768")
                                              .WithHeader("X-FB-Debug", "nHnIdW5fAtGdBkL+q1UqYFrtbWMxcb3zufUOMIWmC1w=")
                               );

                var parameters = new Dictionary<string, object>();
                parameters["_etag_"] = "\"539feb8aee5c3d20a2ebacd02db380b27243b255\"";

                dynamic result = fb.Get("4", parameters);

                Assert.Equal("\"\"539feb8aee5c3d20a2ebacd02db380b27243b255\"\"", fakeRequest.Headers["If-None-Match"]);

                Assert.Equal(HttpStatusCode.NotModified, fakeResponse.StatusCode);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.True(result.Count == 1);
                Assert.True(result.ContainsKey("headers"));
                Assert.False(result.ContainsKey("body"));

                var headers = result.headers;
                Assert.Equal("text/javascript; charset=UTF-8", headers["Content-Type"]);
                Assert.Equal("548768", headers["X-FB-Rev"]);
                Assert.Equal("nHnIdW5fAtGdBkL+q1UqYFrtbWMxcb3zufUOMIWmC1w=", headers["X-FB-Debug"]);

            }

            [Fact]
            public void EmptyETag()
            {
                var fb = new FacebookClient();

                FakeHttpWebRequestWrapper fakeRequest = null;
                FakeHttpWebResponseWrapper fakeResponse = null;

                fb.HttpWebRequestFactory =
                    uri => fakeRequest =
                           new FakeHttpWebRequestWrapper()
                               .WithRequestUri(uri)
                               .FakeResponse(out fakeResponse)
                               .WithResponseStreamAs(
                                   "{\"id\":\"4\",\"name\":\"Mark Zuckerberg\",\"first_name\":\"Mark\",\"last_name\":\"Zuckerberg\",\"link\":\"http:\\/\\/www.facebook.com\\/zuck\",\"username\":\"zuck\",\"gender\":\"male\",\"locale\":\"en_US\"}")
                               .WithContentType("text/javascript; charset=UTF-8")
                               .WithStatusCode(200)
                               .WithHeader("X-FB-Rev", "548768")
                               .WithHeader("X-FB-Debug", "nHnIdW5fAtGdBkL+q1UqYFrtbWMxcb3zufUOMIWmC1w=")
                               .GetFakeHttpWebRequestWrapper();

                var parameters = new Dictionary<string, object>();
                parameters["_etag_"] = "";

                dynamic result = fb.Get("4", parameters);

                Assert.Equal("GET", fakeRequest.Method);
                Assert.Equal("https://graph.facebook.com/4", fakeRequest.RequestUri.AbsoluteUri);
                Assert.True(fakeRequest.ContentLength == 0);

                Assert.False(fakeRequest.Headers.AllKeys.Contains("If-None-Match"));
                Assert.Equal(HttpStatusCode.OK, fakeResponse.StatusCode);

                Assert.IsAssignableFrom<IDictionary<string, object>>(result);
                Assert.True(result.Count == 2);
                Assert.True(result.ContainsKey("headers"));
                Assert.True(result.ContainsKey("body"));

                var headers = result.headers;
                Assert.Equal("text/javascript; charset=UTF-8", headers["Content-Type"]);
                Assert.Equal("548768", headers["X-FB-Rev"]);
                Assert.Equal("nHnIdW5fAtGdBkL+q1UqYFrtbWMxcb3zufUOMIWmC1w=", headers["X-FB-Debug"]);

                var body = result.body;

                Assert.IsAssignableFrom<IDictionary<string, object>>(body);
                Assert.Equal("4", body.id);
                Assert.Equal("Mark Zuckerberg", body.name);
            }


        }
    }
}