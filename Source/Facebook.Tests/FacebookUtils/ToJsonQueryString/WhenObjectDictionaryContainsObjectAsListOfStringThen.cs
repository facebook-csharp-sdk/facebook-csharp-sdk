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

namespace Facebook.Tests.FacebookUtils.ToJsonQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class WhenObjectDictionaryContainsObjectAsListOfStringThen
    {
        [Fact]
        public void ItShouldBeDecodedWithSquareBrackets()
        {
            var dict = new Dictionary<string, object>
                           {
                               {"key1", "value1"},
                               {"key2", "value2"},
                               {"key3", new List<string> {"list_item1", "list_item2"}}
                           };

            // key1=value1&key2=value2&key3=["list_item1","list_item2"]
            var expected = "key1=value1&key2=value2&key3=%5B%22list_item1%22%2C%22list_item2%22%5D";

            var result = FacebookUtils.ToJsonQueryString(dict);

            Assert.Equal(expected, result);
        }
    }
}