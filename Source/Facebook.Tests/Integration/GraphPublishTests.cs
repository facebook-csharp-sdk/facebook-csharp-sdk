// --------------------------------
// <copyright file="GraphPublishTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Configuration;
using System.Dynamic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests.Graph
{

    [TestClass]
    public class GraphPublishTests
    {

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Wall_Post_Publish()
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.message = "This is a test message that has been published by the Facebook C# SDK on Codeplex. " + DateTime.UtcNow.Ticks.ToString();
            parameters.attribution = "Facebook C# SDK";

            var result = app.Api("/me/feed", parameters, HttpMethod.Post);

            Assert.AreNotEqual(null, result.id);
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Wall_Post_Publish_And_Delete()
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.message = "This is a test message that has been published by the Facebook C# SDK on Codeplex. " + DateTime.UtcNow.Ticks.ToString();

            var result = app.Api("/me/feed", parameters, HttpMethod.Post);

            Assert.AreNotEqual(null, result.id);

            // Delete methods should return 'true'
            var isDeleted = app.Api(result.id, HttpMethod.Delete);

            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Publish_Photo_To_Existing_Album()
        {
#if DEBUG
            string photoPath = @"..\..\..\Facebook.Tests\bin\Debug\monkey.jpg";
#else
            string photoPath = @"..\..\..\Facebook.Tests\bin\Release\monkey.jpg";
#endif
            string albumId = ConfigurationManager.AppSettings["AlbumId"];
            byte[] photo = File.ReadAllBytes(photoPath);

            FacebookApp app = new FacebookApp();
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

            dynamic result = app.Api(String.Format("/{0}/photos", albumId), parameters, HttpMethod.Post);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(null, result.id);
        }

    }


}
