
namespace Facebook.Tests.ExceptionFactory.GetGraphException.Object
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GivenANonEmptyDictionaryOfStringObjectThatDoesNotContainErrorKeyThen
    {
        [Fact]
        public void ResultIsNotNull()
        {
            var dictWithoutErrorCodeKey = new Dictionary<string, object> { { "dummy_key", "dummy_value" } };

            var result = ExceptionFactory.GetGraphException(dictWithoutErrorCodeKey);

            Assert.Null(result);
        }
    }
}