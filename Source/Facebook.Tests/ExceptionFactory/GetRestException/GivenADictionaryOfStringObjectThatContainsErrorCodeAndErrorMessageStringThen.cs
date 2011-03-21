
namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryOfStringObjectThatContainsErrorCodeAndErrorMessageStringThen
    {
        private IDictionary<string, object> dict;
        private int errorCode;
        private string errorMessage;

        public GivenADictionaryOfStringObjectThatContainsErrorCodeAndErrorMessageStringThen()
        {
            errorCode = 100;
            errorMessage = "The parameter fields is required";
            dict = new Dictionary<string, object>
                       {
                           {"error_code", errorCode},
                           {"error_msg", errorMessage}
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

        [Fact]
        public void ErrorMessageIsSetCorrectly()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.Equal("(100) The parameter fields is required", result.Message);
        }
    }
}