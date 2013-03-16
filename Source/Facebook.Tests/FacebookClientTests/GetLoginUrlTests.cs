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
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GetLoginUrlTests
    {
        private readonly FacebookClient _fb;

        public GetLoginUrlTests()
        {
            _fb = new FacebookClient();
        }

        [Fact]
        public void GivenParametersAsCodeTokenThenShouldEncodeCorrectly()
        {
            var loginParameters = new Dictionary<string, object>();
            loginParameters["client_id"] = "appid";
            loginParameters["client_secret"] = "clientsecret";
            loginParameters["response_type"] = "code token";
            loginParameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            var loginUrl = _fb.GetLoginUrl(loginParameters);

            Assert.Equal("https://www.facebook.com/dialog/oauth?client_id=appid&client_secret=clientsecret&response_type=code%20token&redirect_uri=https%3A%2F%2Fwww.facebook.com%2Fconnect%2Flogin_success.html",
                loginUrl.AbsoluteUri);
        }

        [Fact]
        public void AddsClientIdCorrectlyWhenSetOnFacebookClient()
        {
            _fb.AppId = "appid";
            var loginParameters = new Dictionary<string, object>();
            loginParameters["client_secret"] = "clientsecret";
            loginParameters["response_type"] = "code token";
            loginParameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            var loginUrl = _fb.GetLoginUrl(loginParameters);

            Assert.Equal("https://www.facebook.com/dialog/oauth?client_secret=clientsecret&response_type=code%20token&redirect_uri=https%3A%2F%2Fwww.facebook.com%2Fconnect%2Flogin_success.html&client_id=appid",
                loginUrl.AbsoluteUri);
        }

        [Fact]
        public void GivenParameterAsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _fb.GetLoginUrl(null));
        }

        public class ContainsMobileInParameter
        {
            private readonly FacebookClient _fb;

            public ContainsMobileInParameter()
            {
                _fb = new FacebookClient();
            }

            [Fact]
            public void MobileIsTrueThen_ShouldGenerateMobileLoginUrl()
            {
                var loginParameters = new Dictionary<string, object>();
                loginParameters["mobile"] = true;

                var loginUrl = _fb.GetLoginUrl(loginParameters);

                Assert.Equal("https://m.facebook.com/dialog/oauth", loginUrl.AbsoluteUri);
            }

            [Fact]
            public void MobileIsFalseThen_ShouldGenerateNonMobileLoginUrl()
            {
                var loginParameters = new Dictionary<string, object>();
                loginParameters["mobile"] = false;

                var loginUrl = _fb.GetLoginUrl(loginParameters);

                Assert.Equal("https://www.facebook.com/dialog/oauth", loginUrl.AbsoluteUri);
            }
        }
    }
}