namespace Facebook.Utils.Tests
{
    using Xunit;
    using Xunit.Extensions;

    public class RemoveTrailingSlashTests
    {
        [InlineData("http://www.ntotten.com/", "http://www.ntotten.com")]
        [InlineData("jimzimmerman.com/", "jimzimmerman.com")]
        [InlineData("www.prabir.me/", "www.prabir.me")]
        [Theory(DisplayName = "RemoveTrailingSlash: Given a url with trailing slash and no querystring Then it should remove the trailing slash")]
        public void RemoveTrailingSlash_GivenAUrlWithTrailingSlashAndNoQuerystring_ThenItShouldRemoveTheTrailingSlash(string inputUrl, string expectedUrl)
        {
            var result = FacebookUtils.RemoveTrailingSlash(inputUrl);

            Assert.Equal(expectedUrl, result);
        }

        [Fact(DisplayName = "RemoveTrailingSlash: Given a null url Then it should return empty string")]
        public void RemoveTrailingSlash_GivenANullUrl_ThenItShouldReturnEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash((string)null);

            Assert.Equal(string.Empty, result);
        }

        [Fact(DisplayName = "RemoveTrailingSlash: Given an empty string Then it should return empty string")]
        public void RemoveTrailingSlash_GivenAnEmptyString_ThenItShouldReturnEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash("");

            Assert.Equal(string.Empty, result);
        }

        [InlineData("1", "1")]
        [InlineData("2", "2")]
        [InlineData("!", "!")]
        [InlineData("a", "a")]
        [Theory(DisplayName = "RemoveTrailingSlash: Given a url with length 1 which is not slash Then it should be the same as input")]
        public void RemoveTrailingSlash_GivenAUrlWithLength1WhichIsNotSlash_ThenItShouldBeTheSameAsInput(string input, string expected)
        {
            var result = FacebookUtils.RemoveTrailingSlash(input);

            Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "RemoveTrailingSlash: Given a string as slash Then the result should be empty string")]
        public void RemoveTrailingSlash_GivenAStringAsSlash_ThenTheResultShouldBeEmptyString()
        {
            var result = FacebookUtils.RemoveTrailingSlash("/");

            Assert.Equal(string.Empty, result);
        }
    }
}