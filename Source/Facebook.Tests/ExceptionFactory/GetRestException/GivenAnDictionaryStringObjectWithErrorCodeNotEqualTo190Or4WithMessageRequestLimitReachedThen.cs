namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenAnDictionaryStringObjectWithErrorCodeNotEqualTo190Or4WithMessageRequestLimitReachedThen
    {
        private IDictionary<string, object> dict;

        public GivenAnDictionaryStringObjectWithErrorCodeNotEqualTo190Or4WithMessageRequestLimitReachedThen()
        {
            int errorCodeNot190or4 = 10000;
            dict = new Dictionary<string, object>
                       {
                           { "error_code", errorCodeNot190or4 },
                           { "error_msg", "request limit reached"}
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
    }
}