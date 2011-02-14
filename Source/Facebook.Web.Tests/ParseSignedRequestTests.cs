/*
namespace Facebook
{
    using System;
    using Facebook.Web;
    using Xunit;

    namespace ParseSignedRequestTests
    {
        public class EncryptedParseSignedRequestTests
        {

            [Fact(DisplayName = "Parse: Given a valid signed request Then it should extract the expires in correctly")]
            public void Parse_GivenAValidSignedRequest_ThenItShouldExtractTheExpiresInCorrectly()
            {
                var signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

                string secret = "13750c9911fec5865d01f3bd00bdf4db";
                int maxAge = 3600;
                double currentTime = 1287601970;
                var expiresInUnixTime = 6412;
                var expectedTime = FacebookUtils.FromUnixTime(expiresInUnixTime);

                var result = FacebookSignedRequestOld.Parse(secret, signedRequest, maxAge, currentTime);

                Assert.Equal(expectedTime, result.Expires);
            }

            [Fact(DisplayName = "Parse: Given a valid signed request Then it should extract the user id correctly")]
            public void Parse_GivenAValidSignedRequest_ThenItShouldExtractTheUserIdCorrectly()
            {
                var signedRequest = "t63pZQ4Q3ZTHJt0hOsKrY2pb28xRlduW0pg4lL_Zhl4.eyJhbGdvcml0aG0iOiJBRVMtMjU2LUNCQyBITUFDLVNIQTI1NiIsImlzc3VlZF9hdCI6MTI4NzYwMTk4OCwiaXYiOiJmRExKQ1cteWlYbXVOYTI0ZVNhckpnIiwicGF5bG9hZCI6IllHeW00cG9Rbk1UckVnaUFPa0ZUVkk4NWxsNVJ1VWlFbC1JZ3FmeFRPVEhRTkl2VlZJOFk4a1Z1T29lS2FXT2Vhc3NXRlRFdjBRZ183d0NDQkVlbjdsVUJCemxGSjFWNjNISjNBZjBTSW5nY3hXVEo3TDZZTGF0TW13WGdEQXZXbjVQc2ZxeldrNG1sOWg5RExuWXB0V0htREdMNmlCaU9oTjdXeUk3cDZvRXBWcmlGdUp3X2NoTG9QYjhhM3ZHRG5vVzhlMlN4eDA2QTJ4MnhraWFwdmcifQ";

                string secret = "13750c9911fec5865d01f3bd00bdf4db";
                int maxAge = 3600;
                double currentTime = 1287601970;

                var result = FacebookSignedRequestOld.Parse(secret, signedRequest, maxAge, currentTime);

                Assert.Equal("499091902", result.UserId);
            }
        }

        public class OldParseSignedRequestTests
        {
            [Fact(DisplayName = "Parse: Given a signed request value with more than 1 dot Then it should throw InvalidOperationException")]
            public void Parse_GivenASignedRequestValueWithMoreThan1Dot_ThenItShouldThrowInvalidOperationException()
            {
                var signedRequest = "invalid.signed.request.with.more.than.two.dots";
                string secret = "secret";

                Assert.Throws<InvalidOperationException>(() => FacebookSignedRequestOld.Parse(secret, signedRequest));
            }

            [Fact(DisplayName = "Parse: Given a signed request value without signature Then it should throw InvalidOperationException")]
            public void Parse_GivenASignedRequestValueWithoutSignature_ThenItShouldThrowInvalidOperationException()
            {
                var signedRequest = ".envelope_only";
                string secret = "secret";

                Assert.Throws<InvalidOperationException>(() => FacebookSignedRequestOld.Parse(secret, signedRequest));
            }

            [Fact(DisplayName = "Parse: Given a signed request value without envelope Then it should throw InvalidOperationException")]
            public void Parse_GivenASignedRequestValueWithoutEnvelope_ThenItShouldThrowInvalidOperationException()
            {
                var signedRequest = "signed_request_only.";
                string secret = "secret";

                Assert.Throws<InvalidOperationException>(() => FacebookSignedRequestOld.Parse(secret, signedRequest));
            }

            [Fact(DisplayName = "Parse: Given a signed request that contains valid signature Then it doesnot throw InvalidOperationException")]
            public void Parse_GivenASignedRequestThatContainsValidSignature_ThenItDoesnotThrowInvalidOperationException()
            {
                var signedRequest = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
                var secret = "543690fae0cd186965412ac4a49548b5";

                Assert.DoesNotThrow(() => FacebookSignedRequestOld.Parse(secret, signedRequest));
            }

            [Fact(DisplayName = "Parse: Given a valid signed request and invalid secret key Then it should throw InvalidOperationException")]
            public void Parse_GivenAValidSignedRequestAndInvalidSecretKey_ThenItShouldThrowInvalidOperationException()
            {
                var signedRequest = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
                var secret = "invalid_secret";

                Assert.Throws<InvalidOperationException>(() => FacebookSignedRequestOld.Parse(secret, signedRequest));
            }

            [Fact(DisplayName = "Parse: Given a valid signed request Then it should extract the access token correctly")]
            public void Parse_GivenAValidSignedRequest_ThenItShouldExtractTheAccessTokenCorrectly()
            {
                var signedRequest = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
                var secret = "543690fae0cd186965412ac4a49548b5";

                var result = FacebookSignedRequestOld.Parse(secret, signedRequest);

                Assert.Equal("120625701301347|2.I3WPFn_9kJegQNDf5K_I2g__.3600.1282928400-14812017|qrfiOepbv4fswcdYtRWfANor9bQ.", result.AccessToken);
            }

            [Fact(DisplayName = "Parse: Given a valid signed request Then it should extract the user id correctly")]
            public void Parse_GivenAValidSignedRequest_ThenItShouldExtractTheUserIdCorrectly()
            {
                var signedRequest = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
                var secret = "543690fae0cd186965412ac4a49548b5";

                var result = FacebookSignedRequestOld.Parse(secret, signedRequest);

                Assert.Equal("14812017", result.UserId);
            }

            [Fact(DisplayName = "Parse: Given a valid signed request Then it should extract the expires in correctly")]
            public void Parse_GivenAValidSignedRequest_ThenItShouldExtractTheExpiresInCorrectly()
            {
                var signedRequest = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";

                string secret = "543690fae0cd186965412ac4a49548b5";
                var expiresInUnixTime = 1282928400;
                var expectedTime = FacebookUtils.FromUnixTime(expiresInUnixTime);

                var result = FacebookSignedRequestOld.Parse(secret, signedRequest);

                Assert.Equal(expectedTime, result.Expires);
            }
        }
    }
}*/