namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQuerystringAsWhitespaceThen
    {
        [Fact]
        public void ResultShouldNotBeNull()
        {
            string whiteSpaceQueryString = "    ";
            var result = FacebookUtils.ParseUrlQueryString(whiteSpaceQueryString);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            string whiteSpaceQueryString = "    ";
            var result = FacebookUtils.ParseUrlQueryString(whiteSpaceQueryString);

            Assert.Equal(0, result.Count);
        }
    }
}