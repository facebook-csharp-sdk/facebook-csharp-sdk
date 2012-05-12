//-----------------------------------------------------------------------
// <copyright file="ParseSignedRequestTests.cs" company="The Outercurve Foundation">
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
    using System.Collections.Generic;

    public class ParseSignedRequestTests
    {
        private const string SignedRequest = "GlHPe9RZ_mLEb9W81pX-LxWrNcnE_hD14AdllYvqRYg.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEzMzY4NDU2MDAsImlzc3VlZF9hdCI6MTMzNjg0MTkzOCwib2F1dGhfdG9rZW4iOiJBQUFCM2dyZlRyWHdCQUlZbXNJREtiZ2VwS2RMNk01SUszdjRwTUdBaTZPRUtXTHpYOTFiWkJDNFpBVHphZGlMbmJLNGs4Q0JyU2JvNVpDcVc1YTdhWkEzRjVEU0hNSWgzV2Fybk5WTFJHVGcyVFdMYnBKNHoiLCJ1c2VyIjp7ImNvdW50cnkiOiJ1cyIsImxvY2FsZSI6ImVuX1VTIiwiYWdlIjp7Im1pbiI6MjF9fSwidXNlcl9pZCI6IjEwMDAwMTMyNzY0MjAyNiJ9";
        private const string AppSecret = "25c88846f5190930e9cd8921f20fa7e7";

        [Fact]
        public void CorrectlyParsesSignedRequest()
        {
            var fb = new FacebookClient();

            var signedRequest = (IDictionary<string, object>)fb.ParseSignedRequest(AppSecret, SignedRequest);

            Assert.IsAssignableFrom<IDictionary<string, object>>(signedRequest);
            Assert.IsType<JsonObject>(signedRequest);

            Assert.Equal("HMAC-SHA256", signedRequest["algorithm"]);
            Assert.Equal(1336845600L, signedRequest["expires"]);
            Assert.Equal(1336841938L, signedRequest["issued_at"]);
            Assert.Equal("AAAB3grfTrXwBAIYmsIDKbgepKdL6M5IK3v4pMGAi6OEKWLzX91bZBC4ZATzadiLnbK4k8CBrSbo5ZCqW5a7aZA3F5DSHMIh3WarnNVLRGTg2TWLbpJ4z", signedRequest["oauth_token"]);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionIfAppSecretIsNull()
        {
            var fb = new FacebookClient();

            Assert.Throws<ArgumentNullException>(() => fb.ParseSignedRequest(null, SignedRequest));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionIfAppSecretIsEmpty()
        {
            var fb = new FacebookClient();

            Assert.Throws<ArgumentNullException>(() => fb.ParseSignedRequest(string.Empty, SignedRequest));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionIfSignedRequestIsNull()
        {
            var fb = new FacebookClient();

            Assert.Throws<ArgumentNullException>(() => fb.ParseSignedRequest(AppSecret, null));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionIfSignedRequestIsEmpty()
        {
            var fb = new FacebookClient();

            Assert.Throws<ArgumentNullException>(() => fb.ParseSignedRequest(AppSecret, string.Empty));
        }
    }
}