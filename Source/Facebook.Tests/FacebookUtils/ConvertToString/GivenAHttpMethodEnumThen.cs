namespace Facebook.Tests.FacebookUtils.ConvertToString
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAHttpMethodEnumThen
    {
#if SILVERLIGHT
        [InlineData(HttpMethod.Get, "GET")]
        [InlineData(HttpMethod.Post, "POST")]
        [Theory]
        public void ItShouldReturnTheEquivalentString(HttpMethod httpMethod, string strHttpMethod)
        {
            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }

        [Fact]
        public void DeleteShouldReturnPost()
        {
            var result = FacebookUtils.ConvertToString(HttpMethod.Delete);
            Assert.Equal("POST", result);
        }
#else
        [InlineData(HttpMethod.Get, "GET")]
        [InlineData(HttpMethod.Post, "POST")]
        [InlineData(HttpMethod.Delete, "DELETE")]
        [Theory]
        public void ItShouldReturnTheEquivalentString(HttpMethod httpMethod, string strHttpMethod)
        {
            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }
#endif
    }
}