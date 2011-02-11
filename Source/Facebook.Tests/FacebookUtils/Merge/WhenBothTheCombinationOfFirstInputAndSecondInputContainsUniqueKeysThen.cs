namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenBothTheCombinationOfFirstInputAndSecondInputContainsUniqueKeysThen
    {
        [Fact]
        public void ResultShouldNotBeNull()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBeEqualToCountOfFirstAndSecondInputs()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TheValuesShouldBeEqualToTheOneInsertedFromFirstOrSecond()
        {
            var first = new Dictionary<string, object> { { "prop1", "value1" } };
            var second = new Dictionary<string, object> { { "prop2", "value2" } };

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(first["prop1"], result["prop1"]);
            Assert.Equal(second["prop2"], result["prop2"]);
        }
    }
}