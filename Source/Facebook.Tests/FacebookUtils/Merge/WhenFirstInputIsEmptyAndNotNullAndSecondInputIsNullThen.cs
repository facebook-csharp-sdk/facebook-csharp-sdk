namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenFirstInputIsEmptyAndNotNullAndSecondInputIsNullThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = new Dictionary<string, object>();
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfTheResultShouldBe0()
        {
            IDictionary<string, object> first = new Dictionary<string, object>();
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(0, result.Count);
        }
    }
}