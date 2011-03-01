namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenADictionaryOfStringObjectWithErrorCode190Then
    {
        private IDictionary<string, object> dict;
        private int errorCode;

        public GivenADictionaryOfStringObjectWithErrorCode190Then()
        {
            errorCode = 190;
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
        public void ResultIsOfTypeFacebookOAuthException()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.IsType<FacebookOAuthException>(result);
        }

        [Fact]
        public void ErrorTypeIs190()
        {
            var result = ExceptionFactory.GetRestException(dict);

            Assert.Equal("190", result.ErrorType);
        }
    }
}