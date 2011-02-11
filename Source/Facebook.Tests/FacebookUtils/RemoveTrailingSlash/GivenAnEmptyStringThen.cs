namespace Facebook.Tests.FacebookUtils.RemoveTrailingSlash
{
    using Facebook;
    using Xunit;

    public class GivenAnEmptyStringThen
    {
        [Fact]
        public void ItShouldReturnEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash("");

            Assert.Equal(string.Empty, result);
        }
    }
}