// --------------------------------
// <copyright file="RestPublishTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Tests.Rest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Dynamic;
    using System.IO;
    using Xunit;

    public class RestPublishTests
    {

        [Fact]
        // [TestCategory("RequiresOAuth")]
        public void publish_photo_to_application_album()
        {

#if DEBUG
            string photoPath = @"..\..\..\Facebook.Tests\bin\Debug\monkey.jpg";
#else
            string photoPath = @"..\..\..\Facebook.Tests\bin\Release\monkey.jpg";
#endif

            byte[] photo = File.ReadAllBytes(photoPath);
            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.caption = "This is a test photo of a monkey that has been uploaded " +
                                 "by the Facebook C# SDK (http://facebooksdk.codeplex.com)" +
                                 "using the REST API";
            parameters.method = "facebook.photos.upload";
            parameters.uid = ConfigurationManager.AppSettings["UserId"];
            var mediaObject = new FacebookMediaObject
            {
                FileName = "monkey.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;
            dynamic result = app.Post(parameters);

            Assert.NotNull(result);
            Assert.NotEqual(result.aid, null);
        }


        [Fact]
        public void Publish_Global_News()
        {
            FacebookClient app = new FacebookClient();
            dynamic parameters = new ExpandoObject();
            parameters.method = "dashboard.addGlobalNews";

            var list = new List<object>();
            dynamic news1 = new ExpandoObject();
            news1.message = "This is a test news message. " + DateTime.UtcNow.Ticks.ToString();
            list.Add(news1);

            parameters.news = list;

            dynamic result = app.Post(parameters);

            long id;
            long.TryParse(result, out id);
            Assert.True(id > 0);
        }
    }
}
