
namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryOfStringObjectThatContainsErrorCodeStringThen
    {
        private IDictionary<string, object> dict;
        private int errorCode;

        public GivenADictionaryOfStringObjectThatContainsErrorCodeStringThen()
        {
            errorCode = 100;
            dict = new Dictionary<string, object>
                       {
                           {"error_code", errorCode},
                           {"error_msg", "The parameter fields is required"}
                           // ommited other fields for brevity
                       };
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsOfTypeFacebookApiException()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.IsType<FacebookApiException>(result);
        }

        [Fact]
        public void ErrorTypeIsEqualToErrorCode()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.Equal(errorCode.ToString(), result.ErrorType);
        }
    }
}