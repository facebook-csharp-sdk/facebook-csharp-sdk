namespace Facebook.Tests.FacebookClient
{
    using System;
    using Xunit;
    using Facebook;

    public class FacebookClientTests
    {
        [Fact]
        public void A()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Batch(
                new FacebookBatchParameter("me", new { fields = new[] { "id", "name" }, pic = new FacebookMediaObject() }) { Data = new { method = "POST", return_ssl_resources = "true" } });

        }
        public class ConsutructorTests
        {
            public class DefaultConstructorTests
            {
                private FacebookClient _fb;

                public DefaultConstructorTests()
                {
                    _fb = new FacebookClient();
                }

                [Fact]
                public void IsSecureConnectionIsFalse()
                {
                    Assert.False(_fb.IsSecureConnection);
                }

                [Fact]
                public void UseFacebookShouldBetaIsFalse()
                {
                    Assert.False(_fb.UseFacebookBeta);
                }

                [Fact]
                public void AccessTokenIsNull()
                {
                    Assert.Null(_fb.AccessToken);
                }

                [Fact]
                public void BoundaryIsNull()
                {
                    Assert.Null(_fb.Boundary);
                }

                [Fact]
                public void SerializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.SerializeJson);
                }

                [Fact]
                public void DeserializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.DeserializeJson);
                }

                [Fact]
                public void HttpWebRequestFactoryIsNull()
                {
                    Assert.Null(_fb.HttpWebRequestFactory);
                }
            }

            public class ConstructorAccessTokenTests
            {
                private FacebookClient _fb;

                public ConstructorAccessTokenTests()
                {
                    _fb = new FacebookClient("dummy_access_token");
                }

                [Fact]
                public void IsSecureConnectionIsFalse()
                {
                    Assert.False(_fb.IsSecureConnection);
                }

                [Fact]
                public void UseFacebookShouldBetaIsFalse()
                {
                    Assert.False(_fb.UseFacebookBeta);
                }

                [Fact]
                public void AcessTokenIsTheSameAsTheOnePassedInTheConstructor()
                {
                    Assert.Equal("dummy_access_token", _fb.AccessToken);
                }

                [Fact]
                public void BoundaryIsNull()
                {
                    Assert.Null(_fb.Boundary);
                }

                [Fact]
                public void SerializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.SerializeJson);
                }

                [Fact]
                public void DeserializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.DeserializeJson);
                }

                [Fact]
                public void HttpWebRequestFactoryIsNull()
                {
                    Assert.Null(_fb.HttpWebRequestFactory);
                }

                public class ConstructorAccessTokenIsNull
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => new FacebookClient((string)null));
                    }
                }

                public class ContructorAccessTokenIsEmpty
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => new FacebookClient(string.Empty));
                    }
                }
            }

            public class ConstructorAppIdAppSecret
            {
                [Fact]
                public void ThrowsArgumentNullExceptionIfAppIdIsEmpty()
                {
                    Assert.Throws<ArgumentNullException>(() => new FacebookClient(null, "appsecret"));
                }

                [Fact]
                public void ThrowsArgumentNullExceptionIfAppIdIsNull()
                {
                    Assert.Throws<ArgumentNullException>(() => new FacebookClient(null, "appsecret"));
                }

                [Fact]
                public void ThrowsArgumentNullExceptionIfAppSecretIsEmpty()
                {
                    Assert.Throws<ArgumentNullException>(() => new FacebookClient("appid", string.Empty));
                }

                [Fact]
                public void ThrowsArgumentNullExceptionIfAppSecretIsNull()
                {
                    Assert.Throws<ArgumentNullException>(() => new FacebookClient("appid", null));
                }

                [Fact]
                public void CorrectlySetsAccessTokenIfAppIdAndAppSecretAreBothNotNullOrEmpty()
                {
                    var fb = new FacebookClient("appid", "appsecret");
                    Assert.Equal("appid|appsecret", fb.AccessToken);
                }

            }
        }
    }
}