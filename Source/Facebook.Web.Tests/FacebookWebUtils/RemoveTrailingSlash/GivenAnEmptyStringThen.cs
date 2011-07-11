namespace Facebook.Web.Tests.FacebookWebUtils.RemoveTrailingSlash
{
    using Facebook.Web;
    using Xunit;

    public class GivenAnEmptyStringThen
    {
        [Fact]
        public void ItShouldReturnEmptyString()
        {
            var result = FacebookWebUtils.RemoveTrailingSlash("");

            Assert.Equal(string.Empty, result);
        }
    }
}