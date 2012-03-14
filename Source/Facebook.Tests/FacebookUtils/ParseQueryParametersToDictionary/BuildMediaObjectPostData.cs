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

namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Dynamic;
    using System.IO;
    using Facebook;
    using Xunit;

    public class BuildMediaObjectPostData
    {
        [Fact(Skip = "fix test")]
        public void Test()
        {
            /*
            // TODO: give proper name for this test.
            var assmebly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assmebly.GetManifestResourceStream("Facebook.Tests.monkey.jpg");
            byte[] photo = new byte[stream.Length];
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                photo = ms.ToArray();
            }

            dynamic parameters = new ExpandoObject();
            parameters.message = "This is a test photo of a monkey that has been uploaded " +
                                 "by the Facebook C# SDK (http://facebooksdk.codeplex.com)" +
                                 "using the Graph API";
            var mediaObject = new FacebookMediaObject
            {
                FileName = "monkey.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;

            string boundary = "8cd62a36054bd4c";
            byte[] actual = FacebookClient.BuildMediaObjectPostData(parameters, boundary);

            Assert.Equal(127231, actual.Length);
             * */
        }
    }
}