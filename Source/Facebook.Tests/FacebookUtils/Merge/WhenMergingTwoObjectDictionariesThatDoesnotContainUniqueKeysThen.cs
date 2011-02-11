namespace Facebook.Tests.FacebookUtils.Merge
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenMergingTwoObjectDictionariesThatDoesnotContainUniqueKeysThen
    {
        [Fact]
        public void TheResultShouldNotBeNull()
        {
            var first = new Dictionary<string, object>
                            {
                                {"prop1", "value1-first"},
                                {"prop2", "value2"}
                            };
            var second = new Dictionary<string, object>
                             {
                                 {"prop1", "value1-second"},
                                 {"prop3", "value3"}
                             };

            var result = FacebookUtils.Merge(first, second);

            Assert.NotNull(result);
        }

        [Fact]
        public void TheCountOfResultShouldBeEqualToNumberOfUniqueKeys()
        {
            var first = new Dictionary<string, object>
                            {
                                {"prop1", "value1-first"},
                                {"prop2", "value2"}
                            };
            var second = new Dictionary<string, object>
                             {
                                 {"prop1", "value1-second"},
                                 {"prop3", "value3"}
                             };
            var expected = 3;

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(expected, result.Count);
        }

        [Fact]
        public void TheValuesOfNonUniqueKeysOfResultShouldBeOverridenBySecond()
        {

            var first = new Dictionary<string, object>
                            {
                                {"prop1", "value1-first"},
                                {"prop2", "value2"}
                            };
            var second = new Dictionary<string, object>
                             {
                                 {"prop1", "value1-second"},
                                 {"prop3", "value3"}
                             };

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(second["prop1"], result["prop1"]);
        }

        [Fact]
        public void TheValueOfUniqueKeysOfResultShouldBeThatOfTheInputOverridenByTheSecondInput()
        {
            var first = new Dictionary<string, object>
                            {
                                {"prop1", "value1-first"},
                                {"prop2", "value2"}
                            };
            var second = new Dictionary<string, object>
                             {
                                 {"prop1", "value1-second"},
                                 {"prop3", "value3"}
                             };

            var result = FacebookUtils.Merge(first, second);

            Assert.Equal(first["prop2"], result["prop2"]);
            Assert.Equal(second["prop3"], result["prop3"]);
        }
    }
}