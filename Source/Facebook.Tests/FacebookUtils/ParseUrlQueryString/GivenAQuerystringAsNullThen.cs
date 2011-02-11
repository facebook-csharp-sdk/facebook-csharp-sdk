namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using Facebook;
    using Xunit;

    public class GivenAQuerystringAsNullThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            var result = FacebookUtils.ParseUrlQueryString(null);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            var result = FacebookUtils.ParseUrlQueryString(null);

            Assert.Equal(0, result.Count);
        }
    }
}