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

using System.Collections.Generic;
using Xunit;

namespace Facebook.Tests.FacebookClient.ExtractMediaObject
{
    public class GivenMoreThanOneMediaObjectsOnlyThen
    {
        private IDictionary<string, object> _parameters;

        public GivenMoreThanOneMediaObjectsOnlyThen()
        {
            _parameters = new Dictionary<string, object>();
            _parameters["file1"] = new FacebookMediaObject();
            _parameters["file2"] = new FacebookMediaObject();
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.NotNull(result);
        }

        [Fact]
        public void ResultCountIs1()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ParametersCountIs1()
        {
            var result = Facebook.FacebookClient.ExtractMediaObjects(_parameters);

            Assert.Equal(0, _parameters.Count);
        }
    }
}