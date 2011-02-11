namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenFirstInputIsNullAndSecondIsEmptyAndNotNullThen
    {
        [Fact]
        public void ResultShouldNotBeNull()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>();

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBe0()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>();

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(0, result.Count);
        }
    }
}