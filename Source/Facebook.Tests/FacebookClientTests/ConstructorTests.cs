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

namespace Facebook.Tests.FacebookClient
{
    using System;
    using Facebook;
    using Xunit;

    public class FacebookClientTests
    {
        public class ConsutructorTests
        {
            public class DefaultConstructorTests
            {
                private FacebookClient _fb;

                public DefaultConstructorTests()
                {
                    _fb = new FacebookClient();
                }

                [Fact]
                public void IsSecureConnectionIsFalse()
                {
                    Assert.False(_fb.IsSecureConnection);
                }

                [Fact]
                public void UseFacebookShouldBetaIsFalse()
                {
                    Assert.False(_fb.UseFacebookBeta);
                }

                [Fact]
                public void AccessTokenIsNull()
                {
                    Assert.Null(_fb.AccessToken);
                }

                [Fact]
                public void BoundaryIsNull()
                {
                    Assert.Null(_fb.Boundary);
                }

                [Fact]
                public void SerializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.SerializeJson);
                }

                [Fact]
                public void DeserializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.DeserializeJson);
                }

                [Fact]
                public void HttpWebRequestFactoryIsNull()
                {
                    Assert.Null(_fb.HttpWebRequestFactory);
                }
            }

            public class ConstructorAccessTokenTests
            {
                private FacebookClient _fb;

                public ConstructorAccessTokenTests()
                {
                    _fb = new FacebookClient("dummy_access_token");
                }

                [Fact]
                public void IsSecureConnectionIsFalse()
                {
                    Assert.False(_fb.IsSecureConnection);
                }

                [Fact]
                public void UseFacebookShouldBetaIsFalse()
                {
                    Assert.False(_fb.UseFacebookBeta);
                }

                [Fact]
                public void AcessTokenIsTheSameAsTheOnePassedInTheConstructor()
                {
                    Assert.Equal("dummy_access_token", _fb.AccessToken);
                }

                [Fact]
                public void BoundaryIsNull()
                {
                    Assert.Null(_fb.Boundary);
                }

                [Fact]
                public void SerializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.SerializeJson);
                }

                [Fact]
                public void DeserializeJsonIsNotNull()
                {
                    Assert.NotNull(_fb.DeserializeJson);
                }

                [Fact]
                public void HttpWebRequestFactoryIsNull()
                {
                    Assert.Null(_fb.HttpWebRequestFactory);
                }

                public class ConstructorAccessTokenIsNull
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => new FacebookClient((string)null));
                    }
                }

                public class ContructorAccessTokenIsEmpty
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => new FacebookClient(string.Empty));
                    }
                }
            }
        }
    }
}