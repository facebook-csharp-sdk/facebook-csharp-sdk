namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryOfStringObjectWithErrorCodeAsApiEcTooManyCallsThen
    {
        private IDictionary<string, object> dict;
        private string errorCode;

        public GivenADictionaryOfStringObjectWithErrorCodeAsApiEcTooManyCallsThen()
        {
            errorCode = "API_EC_TOO_MANY_CALLS";
            dict = new Dictionary<string, object>
                       {
                           { "error_code", errorCode }
                           // others ommited for brevity
                       };
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultIsOfTypeFacebookApiLimitException()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.IsType<FacebookApiLimitException>(result);
        }

        [Fact]
        public void ErrorTypeIsApiEcTooManyCalls()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.Equal(errorCode, result.ErrorType);
        }
    }
}