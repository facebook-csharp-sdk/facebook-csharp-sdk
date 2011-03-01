namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Xunit;

    public class GivenAnEmptyDictionaryOfStringObjectThen
    {
        [Fact]
        public void ResultIsNull()
        {
            var dict = new Dictionary<string, object>();

            var result = Facebook.ExceptionFactory.GetRestException(dict);

            Assert.Null(result);
        }
    }
}