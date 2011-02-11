namespace Facebook.Tests.FacebookUtils.ConvertToString
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAHttpMethodEnumThen
    {
        [InlineData(HttpMethod.Get, "GET")]
        [InlineData(HttpMethod.Post, "POST")]
        [InlineData(HttpMethod.Delete, "DELETE")]
        [Theory]
        public void ItShouldReturnTheEquivalentString(HttpMethod httpMethod, string strHttpMethod)
        {
            var result = FacebookUtils.ConvertToString(httpMethod);

            Assert.Equal(strHttpMethod, result);
        }
    }
}