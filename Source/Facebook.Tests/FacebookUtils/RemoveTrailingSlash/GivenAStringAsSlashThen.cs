namespace Facebook.Tests.FacebookUtils.RemoveTrailingSlash
{
    using Facebook;
    using Xunit;

    public class GivenAStringAsSlashThen
    {
        [Fact]
        public void TheResultShouldBeEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash("/");

            Assert.Equal(string.Empty, result);
        }
    }
}