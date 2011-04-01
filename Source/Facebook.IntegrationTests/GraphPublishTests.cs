// --------------------------------
// <copyright file="GraphPublishTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

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
