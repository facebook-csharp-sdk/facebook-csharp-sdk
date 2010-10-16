using System;
using System.Dynamic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook
{
    public partial class FacebookAppTest
    {

        /// <summary>
        ///A test for BuildMediaObjectPostData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Facebook.dll")]
        public void BuildMediaObjectPostDataTest()
        {
#if DEBUG
            string photoPath = @"..\..\..\Facebook.Tests\bin\Debug\monkey.jpg";
#else
            string photoPath = @"..\..\..\Facebook.Tests\bin\Release\monkey.jpg";
#endif
            byte[] photo = File.ReadAllBytes(photoPath);

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

            string boundary = DateTime.UtcNow.Ticks.ToString("x");
            byte[] actual = FacebookApp_Accessor.BuildMediaObjectPostData(parameters, boundary);

            Assert.AreEqual(127231, actual.Length);
        }

        [TestMethod]
        public void Test_Signed_Request()
        {
            var signed_request = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
            var settings = new FacebookSettings
            {
                ApiKey = "ba096d8171d1f64a3156385b6ff4c549",
                AppId = "120625701301347",
                ApiSecret = "543690fae0cd186965412ac4a49548b5",
            };
            FacebookApp_Accessor app = new FacebookApp_Accessor(settings);
            var signedRequest = app.ParseSignedRequest(signed_request);
            Assert.AreEqual("120625701301347|2.I3WPFn_9kJegQNDf5K_I2g__.3600.1282928400-14812017|qrfiOepbv4fswcdYtRWfANor9bQ.", signedRequest.AccessToken);
        }

    }
}
