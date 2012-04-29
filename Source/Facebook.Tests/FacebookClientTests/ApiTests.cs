namespace Facebook.Tests.FacebookClientTests
{
    using System.Collections.Generic;
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
    }
}