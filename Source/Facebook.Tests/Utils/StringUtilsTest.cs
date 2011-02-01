namespace Facebook.Utils.Tests
{
    using System;
    using Xunit;
    using Xunit.Extensions;

    public class StringUtilsTest
    {
        [InlineData(HttpMethod.Get, "GET")]
        [InlineData(HttpMethod.Post, "POST")]
        [InlineData(HttpMethod.Delete, "DELETE")]
        [Theory(DisplayName = "ConvertToString: Given a HttpMethod enum Then it should return the equivalent string ")]
        public void ConvertToString_GivenAHttpMethodEnum_ThenItShouldReturnTheEquivalentString(HttpMethod httpMethod, string strHttpMethod)
        {
            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }

        [InlineData(0, "GET")]
        [InlineData(1, "POST")]
        [InlineData(2, "DELETE")]
        [Theory(DisplayName = "ConvertToString: Given a number in enum range Then it should return the equivalent string ")]
        public void ConvertToString_GivenANumberInEnumRange_ThenItShouldReturnTheEquivalentString(int number, string strHttpMethod)
        {
            var httpMethod = (HttpMethod)number;

            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }

        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(4)]
        [Theory(DisplayName = "ConvertToString: Given a number out of HttpMethod range Then it should throw InvalidOperationException")]
        public void ConvertToString_GivenANumberOutOfHttpMethodRange_ThenItShouldThrowInvalidOperationException(int number)
        {
            var httpMethod = (HttpMethod)number;

            Assert.Throws<InvalidOperationException>(
                () => FacebookUtils.ConvertToString(httpMethod));
        }
    }
}