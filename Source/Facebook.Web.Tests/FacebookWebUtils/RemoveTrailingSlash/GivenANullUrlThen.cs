namespace Facebook.Web.Tests.FacebookWebUtils.RemoveTrailingSlash
{
    using Facebook.Web;
    using Xunit;

    public class GivenANullUrlThen
    {
        [Fact]
        public void ItShouldReturnEmptyString()
        {
            var result = FacebookWebUtils.RemoveTrailingSlash((string)null);

            Assert.Equal(string.Empty, result);
        }
    }
}