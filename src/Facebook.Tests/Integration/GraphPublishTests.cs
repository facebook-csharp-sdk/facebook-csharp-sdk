// --------------------------------
// <copyright file="GraphPublishTests.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace Facebook.Tests.Graph {
    
    [TestClass]
    public class GraphPublishTests {


        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Wall_Post_Publish() {
            //var app = new FacebookApp(TestHelper.FacebookSettings);
            //app.Session = new FacebookSession
            //{
            //    AccessToken = TestHelper.AuthToken,
            //};

            //dynamic parameters = new ExpandoObject();
            //parameters.message = "This is a test wall post.";

            //var postResult = app.Api("/me/feed", parameters, HttpMethod.Post);

            //Assert.IsNotNull(postResult.id);

            //var deleteResult = app.Api(postResult.id, HttpMethod.Delete);

            //Assert.IsTrue(deleteResult);
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void Page_Wall_Post_Publish() {



        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void create_new_album()
        {
            var photoPath = ConfigurationManager.AppSettings["TestPhotoPath"];
            byte[] photo = File.ReadAllBytes(photoPath);
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.caption = "This is a test photo";
            parameters.method = "facebook.photos.upload";
            parameters.uid = ConfigurationManager.AppSettings["UserId"];
            //parameters.aid = "16144";
            //parameters.api_key = "120625701301347";
            //parameters.message = "This is a test photo";
            var mediaObject = new FacebookMediaObject
            {
                FileName = "test.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;
            dynamic result = app.Api(parameters, HttpMethod.Post);
            var id = result.id;
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void publish_photo_to_new_album()
        {
            var photoPath = ConfigurationManager.AppSettings["TestPhotoPath"];
            byte[] photo = File.ReadAllBytes(photoPath);
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.message = "This is a test photo";
            var mediaObject = new FacebookMediaObject
            {
                FileName = "test.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;
           
            dynamic result = app.Api("/16144/photos", parameters, HttpMethod.Post);

            var id = result.id;
        }

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void publish_photo_old()
        {
            var photoPath = ConfigurationManager.AppSettings["TestPhotoPath"];
            byte[] photo = File.ReadAllBytes(photoPath);
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.caption = "This is a test photo";
            parameters.method = "facebook.photos.upload";
            parameters.uid = ConfigurationManager.AppSettings["UserId"];
            parameters.aid = ConfigurationManager.AppSettings["AlbumId"];
            var mediaObject =  new FacebookMediaObject
            {
                FileName = "test.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;


            dynamic result = app.Api(parameters, HttpMethod.Post);
        }

    
    }


}
