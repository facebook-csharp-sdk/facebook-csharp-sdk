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
    using Facebook;
    using Xunit;

    public class GivenAUrlHostIsFacebookGraphWithoutQuerystringAndParameterIsEmptyThen
    {
        [Fact]
        public void CountOfParameterIs0()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(0, parameters.Count);
        }

        [Fact]
        public void ReturnPathEqualsPathWithoutUriHostAndDoesntStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            string originalPathWithoutForwardSlash = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(originalPathWithoutForwardSlash, path);
        }

        [Fact]
        public void ReturnPathDoesNotStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookUtils.ParseQueryParametersToDictionary(url, parameters);

            Assert.NotEqual('/', path[0]);
        }
    }
}