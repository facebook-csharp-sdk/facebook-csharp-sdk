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

namespace Facebook.Tests.FacebookUtils.ParseUrlQueryString
{
    using System.Collections.Generic;
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAValidQuerystringThen
    {
        [PropertyData("ValidQueryStrings")]
        [Theory]
        public void TheCountOfResultShouldBeNumberOfQuerystringKeys(string queryString, int total)
        {
            var result = FacebookUtils.ParseUrlQueryString(queryString);

            Assert.Equal(total, result.Count);
        }

        public static IEnumerable<object[]> ValidQueryStrings
        {
            get
            {
                yield return new object[] { "access_token=124973200873702%7C2.16KX_wTFlY2IAvWucsCKWA__.3600.1294927200-100001327642026%7CERLPsyFd8CP4ZI57VzAn0nl6WXo&expires_in=3699", 2 };
                yield return new object[] { "type=user_agent&client_id=123&redirect_uri=http://www.facebook.com/connect/login_success.html&display=popup", 4 };
                yield return new object[] { "error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.", 3 };
            }
        }
    }
}