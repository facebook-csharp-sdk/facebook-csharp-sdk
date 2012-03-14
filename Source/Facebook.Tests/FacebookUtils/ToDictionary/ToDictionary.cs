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

namespace Facebook.Utils.Tests.ToDictionary
{
    using System.Runtime.Serialization;
    using Xunit;

    public class GivenAObjectThen
    {
        public Post post;

        public GivenAObjectThen()
        {
            this.post = new Post { Message = "dummy message", Privacy = "ALL_FRIENDS" };
        }

        [Fact]
        public void ResultIsNotNull()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.NotNull(parameters);
        }

        [Fact]
        public void ThereAreExactlyTwoKeys()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(2, parameters.Count);
        }

        [Fact]
        public void ContainsMessageKey()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.True(parameters.ContainsKey("message"));
        }

        [Fact]
        public void ContainsPrivacyKey()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.True(parameters.ContainsKey("privacy"));
        }

        [Fact]
        public void MessageShouldBeSetCorrectly()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(this.post.Message, parameters["message"]);
        }

        [Fact]
        public void PrivacyShouldBeSetCorrectly()
        {
            var parameters = FacebookUtils.ToDictionary(this.post);

            Assert.Equal(this.post.Privacy, parameters["privacy"]);
        }

        [DataContract]
        public class Post
        {
            [DataMember(Name = "message")]
            public string Message { get; set; }

            [DataMember(Name = "privacy")]
            public string Privacy { get; set; }
        }
    }
}