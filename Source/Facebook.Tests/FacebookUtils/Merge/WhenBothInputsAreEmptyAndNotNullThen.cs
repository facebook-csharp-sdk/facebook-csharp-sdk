namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenBothInputsAreEmptyAndNotNullThen
    {
        [Fact]
        public void ResultShouldNotBeNull()
        {
            var first = new Dictionary<string, object>();
            var second = new Dictionary<string, object>();

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            var first = new Dictionary<string, object>();
            var second = new Dictionary<string, object>();

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(0, result.Count);
        }
    }
}