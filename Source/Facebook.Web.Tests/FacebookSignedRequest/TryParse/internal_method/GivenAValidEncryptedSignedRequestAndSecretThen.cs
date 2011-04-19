namespace Facebook.Web.Tests.FacebookSignedRequest.TryParse.internal_method
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAValidEncryptedSignedRequestAndSecretThen
    {
        private string signedRequest;
        private string secret;
        private int maxAge;
        private double currentTime;

        public GivenAValidEncryptedSignedRequestAndSecretThen()
        {
            signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

            secret = "13750c9911fec5865d01f3bd00bdf4db";
            maxAge = 3600;
            currentTime = 1287601970;
        }

        [Fact]
        public void ShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true));
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);

            Assert.NotNull(result);
        }

        [Fact]
        public void AlogrithmIsExtractedCorrectly()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);

            Assert.Equal("AES-256-CBC HMAC-SHA256", result["algorithm"]);
        }

        [Fact]
        public void IssuedAtIsExtractedCorrectly()
        {
            long expectedIssutedAt = 1287601988;

            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);

            Assert.Equal(expectedIssutedAt, result["issued_at"]);
        }

        [Fact]
        public void ContainsPayloadKey()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);

            Assert.True(result.ContainsKey("payload"));
        }

        [Fact]
        public void PayloadContainsAccessToken()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);
            var payload = (IDictionary<string, object>)result["payload"];

            Assert.True(payload.ContainsKey("access_token"));
        }

        [Fact]
        public void AccessTokenIsSetCorrectly()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);
            var payload = (IDictionary<string, object>)result["payload"];

            Assert.Equal("101244219942650|2.wdrSr7KyE_VwQ0fjwOfW9A__.3600.1287608400-499091902|XzxMQd-_4tjlC2VEgide4rmg6LI", payload["access_token"]);
        }

        [Fact]
        public void PayloadContainsExpiresIn()
        {
            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);
            var payload = (IDictionary<string, object>)result["payload"];

            Assert.True(payload.ContainsKey("expires_in"));
        }

        [Fact]
        public void ExpiresInIsSetCorrectly()
        {
            long expiresInUnixTime = 6412;

            var result = FacebookSignedRequest.TryParse(secret, signedRequest, maxAge, currentTime, true);
            var payload = (IDictionary<string, object>)result["payload"];

            Assert.Equal(expiresInUnixTime, payload["expires_in"]);
        }
    }
}