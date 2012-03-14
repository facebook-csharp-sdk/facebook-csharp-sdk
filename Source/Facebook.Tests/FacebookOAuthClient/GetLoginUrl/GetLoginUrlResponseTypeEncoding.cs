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

namespace Facebook.Tests.FacebookOAuthClient.GetLoginUrl
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class GetLoginUrlResponseTypeEncoding
    {
        [Fact]
        public void GivenParametersAsCodeTokenThenShouldEncodeCorrectly()
        {
            var oauth = new FacebookOAuthClient();

            var loginParameters = new Dictionary<string, object>();
            loginParameters["client_id"] = "appid";
            loginParameters["client_secret"] = "clientsecret";
            loginParameters["response_type"] = "code token";

            var loginUrl = oauth.GetLoginUrl(loginParameters);

            Assert.Equal("http://www.facebook.com/dialog/oauth/?client_id=appid&client_secret=clientsecret&response_type=code%20token&redirect_uri=http%3A%2F%2Fwww.facebook.com%2Fconnect%2Flogin_success.html",
                loginUrl.AbsoluteUri);
        }
    }
}