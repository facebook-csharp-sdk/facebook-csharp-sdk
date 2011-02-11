namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenFirstInputContainsValuesAndSecondIsNullThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            IDictionary<string, object> first = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheValuesOfResultShouldBeSameAsValuesOfFirstInput()
        {
            IDictionary<string, object> first = new Dictionary<string, object>
                                                    {
                                                        {"prop1", "value1"},
                                                        {"prop2", "value2"}
                                                    };
            IDictionary<string, object> second = null;

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(first["prop1"], result["prop1"]);
            Assert.Equal(first["prop2"], result["prop2"]);
        }
    }
}