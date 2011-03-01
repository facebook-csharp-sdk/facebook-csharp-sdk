namespace Facebook.Tests.ExceptionFactory.GetRestException
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenNonNullInputNotOfTypeIDictionaryStringObjectThen
    {
        [Theory]
        [PropertyData("TestData")]
        public void ResultIsNull(object input)
        {
            var result = ExceptionFactory.GetRestException(input);

            Assert.Null(result);
        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                yield return new object[] { "this_is_not_dictionary<string,object>" };
                yield return new object[] { 1 };
            }
        }
    }
}