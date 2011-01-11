namespace Facebook.Tests
{
    using System;
    using Xunit;

    public class ParseSignedRequestTests
    {
        [Fact(DisplayName = "ParseSignedRequest: Given a signed request value with more than 1 dot Then it should throw InvalidOperationException")]
        public void ParseSignedRequest_GivenASignedRequestValueWithMoreThan1Dot_ThenItShouldThrowInvalidOperationException()
        {
            var signedRequest = "invalid.signed.request.with.more.than.two.dots";
            string secret = "secret";
            int maxAge = 3600;

            Assert.Throws<InvalidOperationException>(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge));
        }

        [Fact(DisplayName = "ParseSignedRequest: Given a signed request value without signature Then it should throw InvalidOperationException")]
        public void ParseSignedRequest_GivenASignedRequestValueWithoutSignature_ThenItShouldThrowInvalidOperationException()
        {
            var signedRequest = ".envelope_only";
            string secret = "secret";
            int maxAge = 3600;

            Assert.Throws<InvalidOperationException>(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge));
        }

        [Fact(DisplayName = "ParseSignedRequest: Given a signed request value without envelope Then it should throw InvalidOperationException")]
        public void ParseSignedRequest_GivenASignedRequestValueWithoutEnvelope_ThenItShouldThrowInvalidOperationException()
        {
            var signedRequest = "signed_request_only.";
            string secret = "secret";
            int maxAge = 3600;

            Assert.Throws<InvalidOperationException>(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge));
        }

        [Fact(DisplayName = "ParseSignedRequest: Given a signed request value older than 1 hour Then it should throw InvalidOperationException")]
        public void ParseSignedRequest_GivenASignedRequestValueOlderThan1Hour_ThenItShouldThrowInvalidOperationException()
        {
            var signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

            string secret = "13750c9911fec5865d01f3bd00bdf4db";
            int maxAge = 3600;
            double currentTime = 1294741460;

            Assert.Throws<InvalidOperationException>(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge, currentTime));
        }

        [Fact(DisplayName = "ParseSignedRequest: Given a signed request that contains valid signature Then it doesnot throw InvalidOperationException")]
        public void ParseSignedRequest_GivenASignedRequestThatContainsValidSignature_ThenItDoesnotThrowInvalidOperationException()
        {
            var signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

            string secret = "13750c9911fec5865d01f3bd00bdf4db";
            int maxAge = 3600;
            double currentTime = 1287601970;

            Assert.DoesNotThrow(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge, currentTime));
        }

        [Fact(DisplayName = "ParseSignedRequest: Given a valid signed request and invalid secret key Then it should throw InvalidOperationException")]
        public void ParseSignedRequest_GivenAValidSignedRequestAndInvalidSecretKey_ThenItShouldThrowInvalidOperationException()
        {
            var signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

            string secret = "invalid_secret";
            int maxAge = 3600;
            double currentTime = 1287601970;

            Assert.Throws<InvalidOperationException>(() => FacebookUtils.ParseSignedRequest(signedRequest, secret, maxAge, currentTime));
        }

    }
}