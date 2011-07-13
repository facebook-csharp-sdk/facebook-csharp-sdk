namespace Facebook.Tests.FacebookUtils.ConvertToString
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenANumberInEnumRangeThen
    {
#if SILVERLIGHT
        [InlineData(0, "GET")]
        [InlineData(1, "POST")]
        [Theory]
        public void ItShouldReturnTheEquivalentString(int number, string strHttpMethod)
        {
            var httpMethod = (HttpMethod)number;

            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }

        [Fact]
        public void TwoShouldReturnPost()
        {
            var result = FacebookUtils.ConvertToString((HttpMethod)2);

            Assert.Equal("POST", result);
        }
#else
        [InlineData(0, "GET")]
        [InlineData(1, "POST")]
        [InlineData(2, "DELETE")]
        [Theory]
        public void ItShouldReturnTheEquivalentString(int number, string strHttpMethod)
        {
            var httpMethod = (HttpMethod)number;

            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }
#endif
    }
}