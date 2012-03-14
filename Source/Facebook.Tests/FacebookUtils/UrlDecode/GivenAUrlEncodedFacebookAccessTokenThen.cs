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

namespace Facebook.Tests.FacebookUtils.UrlDecode
{
    using Facebook;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAUrlEncodedFacebookAccessTokenThen
    {
        [InlineData("124983200973703%7C2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026%7CrPfJfZ38FcwV-8HzRGQdxio9D7B", "124983200973703|2.xZYnCri_odnkuj3xXUNDOA__.3600.1295836200-100001327642026|rPfJfZ38FcwV-8HzRGQdxio9D7B")]
        [InlineData("135972300873702%7C3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025%7Cjbo3zk3aHYVJiLWnKArjERsAU0c", "135972300873702|3.cxZrSyyPVHjISXQCB8MQ_g__.3600.1294833600-100001327642025|jbo3zk3aHYVJiLWnKArjERsAU0c")]
        [Theory(DisplayName = "UrlDecode: Given a url encoded facebook access token Then it should decode correctly")]
        public void ItShouldDecodeCorrectly(string encodedAccessToken, string expectedAccessToken)
        {
            var result = FluentHttp.HttpHelper.UrlDecode(encodedAccessToken);

            Assert.Equal(expectedAccessToken, result);
        }
    }
}