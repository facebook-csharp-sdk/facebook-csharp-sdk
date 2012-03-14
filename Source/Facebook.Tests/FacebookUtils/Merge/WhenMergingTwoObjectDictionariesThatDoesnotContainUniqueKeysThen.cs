//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

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