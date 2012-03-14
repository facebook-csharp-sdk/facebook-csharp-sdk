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
    using System;
    using System.Collections.Generic;
    using Facebook;
    using Xunit;

    public class IfRedirectUriIsEmptyThen
    {
        [Fact]
        public void ItShouldThrowInvalidOperationException()
        {
            var oauth = new FacebookOAuthClient();

            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Assert.Throws<ArgumentException>(() => oauth.GetLoginUrl(parameters));
        }

        [Fact]
        public void GetLoginUri_IfRedirectUriIsEmpty_ThenItExceptionMessageShouldBeRedirectUriRequired()
        {
            var oauth = new FacebookOAuthClient();

            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = "dummy client id";
            parameters["redirect_uri"] = null;

            Exception exception = null;

            try
            {
                oauth.GetLoginUrl(parameters);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.Equal("redirect_uri required.", exception.Message);
        }
    }
}