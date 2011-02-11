namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenBothInputsAreNullThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(0, result.Count);
        }
    }
}