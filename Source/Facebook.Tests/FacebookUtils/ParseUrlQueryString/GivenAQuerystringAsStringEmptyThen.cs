namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQuerystringAsStringEmptyThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            var result = FacebookUtils.ParseUrlQueryString(string.Empty);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            var result = FacebookUtils.ParseUrlQueryString(string.Empty);

            Assert.Equal(0, result.Count);
        }
    }
}