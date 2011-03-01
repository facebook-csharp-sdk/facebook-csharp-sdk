namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Xunit;

    public class GivenANonEmptyDictionaryOfStringObjectThatDoesNotContainErrorCodeKeyThen
    {
        [Fact]
        public void ResultIsNull()
        {
            var dictWithoutErrorCodeKey = new Dictionary<string, object> { { "dummy_key", "dummy_value" } };

            var result = Facebook.ExceptionFactory.GetRestException(dictWithoutErrorCodeKey);

            Assert.Null(result);
        }
    }
}