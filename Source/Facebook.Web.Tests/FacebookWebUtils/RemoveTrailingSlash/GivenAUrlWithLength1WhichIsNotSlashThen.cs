namespace Facebook.Web.Tests.FacebookWebUtils.RemoveTrailingSlash
{
    using Facebook.Web;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlWithLength1WhichIsNotSlashThen
    {
        [InlineData("1", "1")]
        [InlineData("2", "2")]
        [InlineData("!", "!")]
        [InlineData("a", "a")]
        [Theory]
        public void ItShouldBeTheSameAsInput(string input, string expected)
        {
            var result = FacebookWebUtils.RemoveTrailingSlash(input);

            Assert.Equal(expected, result);
        }
    }
}