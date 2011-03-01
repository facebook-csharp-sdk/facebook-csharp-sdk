namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryOfStringObjectWithErrorCode4Then
    {
        private IDictionary<string, object> dict;
        private int errorCode;

        public GivenADictionaryOfStringObjectWithErrorCode4Then()
        {
            errorCode = 4;
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
        public void ErrorTypeIs4()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.Equal("4", result.ErrorType);
        }
    }
}