namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenFirstInputIsNullAndSecondContainsValuesThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                     {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };


            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfTheResultShouldBeEqualToCountOfSecondInput()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                     {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };


            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(second.Count, result.Count);
        }

        [Fact]
        public void TheValuesOfResultShouldBeSameAsValuesOfSecondInput()
        {
            IDictionary<string, object> first = null;
            IDictionary<string, object> second = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(second["prop1"], result["prop1"]);
            Assert.Equal(second["prop2"], result["prop2"]);
        }
    }
}