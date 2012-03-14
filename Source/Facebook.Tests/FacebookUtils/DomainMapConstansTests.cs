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

namespace Facebook.Tests.FacebookUtils
{
    using Facebook;
    using Xunit;

    public class DomainMapConstansTests
    {
        [Fact]
        public void DomainMapApiEqualsApi()
        {
            var result = FacebookUtils.DOMAIN_MAP_API;

            Assert.Equal("api", result);
        }

        [Fact]
        public void DomainMapApiReadIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_API_READ;

            Assert.Equal("api_read", result);
        }

        [Fact]
        public void DomainMapApiVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_API_VIDEO;

            Assert.Equal("api_video", result);
        }

        [Fact]
        public void DomainMapGraphIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_GRAPH;

            Assert.Equal("graph", result);
        }

        [Fact]
        public void DomainMapGraphVideoIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_GRAPH_VIDEO;

            Assert.Equal("graph_video", result);
        }

        [Fact]
        public void DomainMapWwwIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_WWW;

            Assert.Equal("www", result);
        }

        [Fact]
        public void DomainMapAppsIsSetCorrectly()
        {
            var result = FacebookUtils.DOMAIN_MAP_APPS;

            Assert.Equal("apps", result);
        }
    }
}