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

namespace Facebook.Tests.FacebookClientTests
{
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class ParseUrlQueryString
    {
        public class ForceParse
        {
            public class PathIsRelative
            {
                public class ParameterIsNull
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => FacebookClient.ParseUrlQueryString("me", null, true));
                    }
                }

                public class ParameterIsEmpty
                {
                    public class PathIsNull
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString(null, parameters, true);

                            Assert.Equal(string.Empty, result);
                            Assert.Equal(0, parameters.Count);
                        }
                    }

                    public class PathIsEmpty
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString(string.Empty, parameters, true);

                            Assert.Equal(string.Empty, result);
                            Assert.Equal(0, parameters.Count);
                        }
                    }

                    public class PathIsSlash
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("/", parameters, true);

                            Assert.Equal(string.Empty, result);
                            Assert.Equal(0, parameters.Count);
                        }
                    }

                    [Fact]
                    public void CorrectlyParses()
                    {
                        var parameters = new Dictionary<string, object>();
                        var result = FacebookClient.ParseUrlQueryString("me", parameters, true);

                        Assert.Equal("me", result);
                        Assert.Equal(0, parameters.Count);
                    }

                    public class StartsWithSlash
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("/me", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(0, parameters.Count);
                        }
                    }

                    public class ContainsQuerystring
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("me?fields=id,name&access_token=dummy", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }

                        [Fact]
                        public void CorrectlyDecodesQuerystringValues()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("me?fields=id%2Cname&access_token=du%7cmmy",
                                                                            parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("du|mmy", parameters["access_token"]);
                        }

                        [Fact]
                        public void ThrowsArgumentException_IfContainsMoreThenOneQuestionMark()
                        {
                            var parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("?hello=world?hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("me?hello=wor?ld?hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("/me?hello=wor?ld?hi=he", parameters, true));
                        }

                        [Fact]
                        public void ThrowsArgumentExceptoin_IfContainsMoreThenOneEqualsPerKeyValuePair()
                        {
                            var parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("?hello=wor=ld&hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("me?hello=wor=ld&hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("/me?hello&hi=he", parameters, true));
                        }
                    }
                }

                public class ParameterContainsValues
                {
                    public class PathDoesNotContainQuerystring
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id";

                            var result = FacebookClient.ParseUrlQueryString("me", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(1, parameters.Count);
                            Assert.Equal("id", parameters["fields"]);

                        }
                    }

                    public class PathContainsQuerystring
                    {
                        [Fact]
                        public void ParameterOverrides()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id,name";

                            var result = FacebookClient.ParseUrlQueryString("me?fields=id&access_token=dummy", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }
                    }
                }
            }

            public class PathIsAbsolute
            {
                public class ParameterIsNull
                {
                    [Fact]
                    public void ThrowsArgumentNullException()
                    {
                        Assert.Throws<ArgumentNullException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com", null, true));
                    }
                }

                public class ParameterIsEmpty
                {

                    [Fact]
                    public void CorrectlyParses()
                    {
                        var parameters = new Dictionary<string, object>();
                        var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com", parameters, true);

                        Assert.Equal(string.Empty, result);
                        Assert.Equal(0, parameters.Count);
                    }

                    public class EndsWithSlash
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com/", parameters, true);

                            Assert.Equal(string.Empty, result);
                            Assert.Equal(0, parameters.Count);
                        }
                    }

                    public class ContainsQuerystring
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?fields=id,name&access_token=dummy", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }

                        [Fact]
                        public void CorrectlyDecodesQuerystringValues()
                        {
                            var parameters = new Dictionary<string, object>();
                            var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?fields=id%2Cname&access_token=du%7cmmy",
                                                                            parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("du|mmy", parameters["access_token"]);
                        }

                        [Fact]
                        public void ThrowsArgumentException_IfContainsMoreThenOneQuestionMark()
                        {
                            var parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com?hello=world?hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?hello=wor?ld?hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?hello=wor?ld?hi=he", parameters, true));
                        }

                        [Fact]
                        public void ThrowsArgumentExceptoin_IfContainsMoreThenOneEqualsPerKeyValuePair()
                        {
                            var parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com?hello=wor=ld&hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?hello=wor=ld&hi=he", parameters, true));
                            parameters = new Dictionary<string, object>();
                            Assert.Throws<ArgumentException>(() => FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?hello&hi=he", parameters, true));
                        }
                    }
                }

                public class ParameterContainsValues
                {
                    public class PathDoesNotContainQuerystring
                    {
                        [Fact]
                        public void CorrectlyParses()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id";

                            var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(1, parameters.Count);
                            Assert.Equal("id", parameters["fields"]);

                        }
                    }

                    public class PathContainsQuerystring
                    {
                        [Fact]
                        public void ParameterOverrides()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id,name";

                            var result = FacebookClient.ParseUrlQueryString("https://graph.facebook.com/me?fields=id&access_token=dummy", parameters, true);

                            Assert.Equal("me", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }
                    }

                    public class PathIsFacebookLegacyRestApiUrl
                    {
                        [Fact]
                        public void ParameterOverrides()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id,name";

                            var result = FacebookClient.ParseUrlQueryString("https://api.facebook.com/mexs?fields=id&access_token=dummy", parameters, true);

                            Assert.Equal("mexs", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }
                    }

                    public class PathIsNonFacebookUrl
                    {
                        [Fact]
                        public void ParameterOverrides()
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters["fields"] = "id,name";

                            var result = FacebookClient.ParseUrlQueryString("https://www.microsoft.com/msx?fields=id&access_token=dummy", parameters, true);

                            Assert.Equal("msx", result);
                            Assert.Equal(2, parameters.Count);
                            Assert.Equal("id,name", parameters["fields"]);
                            Assert.Equal("dummy", parameters["access_token"]);
                        }
                    }
                }
            }

            public class PathStartsWithQuerystring
            {
                [Fact]
                public void CorrectlyParses()
                {
                    var parameters = new Dictionary<string, object>();
                    var result = FacebookClient.ParseUrlQueryString("?fields=id%2Cname&access_token=du%7cmmy",
                                                                    parameters, true);

                    Assert.Equal(string.Empty, result);
                    Assert.Equal(2, parameters.Count);
                    Assert.Equal("id,name", parameters["fields"]);
                    Assert.Equal("du|mmy", parameters["access_token"]);
                }
            }

            public class PathIsQuerystringWithoutStartingQuestionMark
            {
                [Fact]
                public void CorrectlyParses()
                {
                    var parameters = new Dictionary<string, object>();
                    var result = FacebookClient.ParseUrlQueryString("fields=id%2Cname&access_token=du%7cmmy", parameters,
                                                                    true);

                    Assert.Equal("fields=id%2Cname&access_token=du%7cmmy", result);
                    Assert.Equal(0, parameters.Count);
                }
            }
        }
    }
}