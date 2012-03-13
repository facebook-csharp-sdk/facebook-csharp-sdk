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

namespace Facebook.Tests.Graph
{
    using System;
    using System.Configuration;
    using System.Dynamic;
    using System.IO;
    using Xunit;


    public class GraphPublishTests
    {

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Wall_Post_Publish()
        {
            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.message = "This is a test message that has been published by the Facebook C# SDK on Codeplex. " + DateTime.UtcNow.Ticks.ToString();
            parameters.attribution = "Facebook C# SDK";

            dynamic result = app.Post("/me/feed", parameters);

            Assert.NotEqual(null, result.id);
        }

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Wall_Post_Publish_And_Delete()
        {
            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.message = "This is a test message that has been published by the Facebook C# SDK on Codeplex. " + DateTime.UtcNow.Ticks.ToString();

            dynamic result = app.Post("/me/feed", parameters);

            Assert.NotEqual(null, result.id);

            // Delete methods should return 'true'
            var isDeleted = app.Delete(result.id);

            Assert.True(isDeleted);
        }

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void Publish_Photo_To_Existing_Album()
        {
#if DEBUG
            string photoPath = @"..\..\..\Facebook.Tests\bin\Debug\monkey.jpg";
#else
            string photoPath = @"..\..\..\Facebook.Tests\bin\Release\monkey.jpg";
#endif
            string albumId = ConfigurationManager.AppSettings["AlbumId"];
            byte[] photo = File.ReadAllBytes(photoPath);

            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
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

            dynamic result = app.Post(String.Format("/{0}/photos", albumId), parameters);

            Assert.NotNull(result);
            Assert.NotEqual(null, result.id);
        }

        [Fact]
        public void Publish_Video_To_Wall()
        {
            var videoPath = TestHelpers.GetPathRelativeToExecutable("do-beer-not-drugs.3gp");
            byte[] video = File.ReadAllBytes(videoPath);

            var mediaObject = new FacebookMediaObject
                                  {
                                      FileName = "do-beer-not-drugs.3gp",
                                      ContentType = "video/3gpp"
                                  };
            mediaObject.SetValue(video);

            dynamic parameters = new ExpandoObject();
            parameters.source = mediaObject;
            parameters.method = "video.upload";
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];

            var fb = new FacebookClient();
            dynamic result = fb.Post(parameters);

            Assert.NotNull(result);
            Assert.NotEqual(null, result.vid);
        }
    }
}
