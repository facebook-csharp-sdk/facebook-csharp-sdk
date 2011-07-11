namespace Facebook.Web.Tests.FacebookWebUtils.RemoveTrailingSlash
{
    using Facebook.Web;
    using Xunit;

    public class GivenAStringAsSlashThen
    {
        [Fact]
        public void TheResultShouldBeEmptyString()
        {
            var result = FacebookWebUtils.RemoveTrailingSlash("/");

            Assert.Equal(string.Empty, result);
        }
    }
}