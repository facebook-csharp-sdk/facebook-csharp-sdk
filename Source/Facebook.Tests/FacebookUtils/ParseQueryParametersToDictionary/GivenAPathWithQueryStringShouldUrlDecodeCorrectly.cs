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

namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Collections.Generic;
    using Xunit;

    public class GivenAPathWithQueryStringShouldUrlDecodeCorrectly
    {
        [Fact]
        public void UrlDecodesCorrectly()
        {
            string pathWithQuerystring = "/me?access_token=124973702%7cc6d91d1492d6d1a.1-6306%7cpLl4mEfII18sA";
            var parameters = new Dictionary<string, object>();

            var path = Facebook.FacebookUtils.ParseQueryParametersToDictionary(pathWithQuerystring, parameters);

            Assert.Equal(path, "me");
            Assert.NotNull(parameters);
            Assert.Equal(1, parameters.Count);
            Assert.Equal("124973702|c6d91d1492d6d1a.1-6306|pLl4mEfII18sA", parameters["access_token"]);
        }
    }
}