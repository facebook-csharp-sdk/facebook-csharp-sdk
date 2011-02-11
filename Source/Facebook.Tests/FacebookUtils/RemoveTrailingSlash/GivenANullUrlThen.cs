namespace Facebook.Tests.FacebookUtils.RemoveTrailingSlash
{
    using Facebook;
    using Xunit;

    public class GivenANullUrlThen
    {
        [Fact]
        public void ItShouldReturnEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash((string)null);

            Assert.Equal(string.Empty, result);
        }
    }
}