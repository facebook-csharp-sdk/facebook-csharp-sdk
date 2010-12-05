using System;
using System.Dynamic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Facebook;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Facebook
{
    [TestClass]
    public class FacebookAppTest
    {

        [TestMethod()]
        [DeploymentItem("Facebook.dll")]
        public void Build_Media_Object_Post_Data()
        {
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

        [TestMethod]
        public void Full_Paging_Url_Returns_Correct_Path_And_Parameters()
        {
            string next = "http://graph.facebook.com/me/likes?limit=3&offset=3";
            var parameters = new Dictionary<string, object>();
            var path = FacebookAppBase_Accessor.ParseUrlParameters(next, parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("me/likes", path);
        }

        [TestMethod]
        public void Path_And_Query_Return_Correct_Path_And_Parameters()
        {
            string next = "/me/likes?limit=3&offset=3";
            var parameters = new Dictionary<string, object>();
            var path = FacebookAppBase_Accessor.ParseUrlParameters(next, parameters);
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("me/likes", path);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path as empty string and empty parameters Then count of parameters equals 0")]
        public void ParseUrlParameters_GivenAPathAsEmptyStringAndEmptyParameters_ThenCountOfParametersEquals0()
        {
            string path = string.Empty;
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseUrlParameters(path, parameters);

            Xunit.Assert.Equal(0, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path without querystring and empty parameters Then count of parameters equals 0")]
        public void ParseUrlParameters_GivenAPathWithoutQuerystringAndEmptyParameters_ThenCountOfParametersEquals0()
        {
            var path = "/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseUrlParameters(path, parameters);

            Xunit.Assert.Equal(0, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path with 2 querystrings and empty parameters Then count of parameters equals 2")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenCountOfParametersEquals2()
        {
            string path = "/me/likes?limit=3&offset=3";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseUrlParameters(path, parameters);

            Xunit.Assert.Equal(2, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters:  Given a path with 2 querystrings and empty parameters Then parameter values equal the 2 querystrings")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenParameterValuesEqualThe2Querystrings()
        {
            string path = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseUrlParameters(path, parameters);

            Xunit.Assert.Equal("3", parameters["limit"]);
            Xunit.Assert.Equal("2", parameters["offset"]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path with 2 querystrings and empty parameters Then return path equals the path without querystring")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenReturnPathEqualsThePathWithoutQuerystring()
        {
            string originalPath = "/me/likes?limit=3&offset=3";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseUrlParameters(originalPath, parameters);

            Xunit.Assert.Equal(path, "me/likes");
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path starting with Forward slash and empty parameters Then return path equals the path without forward slash")]
        public void ParseUrlParameters_GivenAPathStartingWithForwardSlashAndEmptyParameters_ThenReturnPathEqualsThePathWithoutForwardSlash()
        {
            string originalPath = "/me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseUrlParameters(originalPath, parameters);

            Xunit.Assert.NotEqual('/', path[0]);
        }
    }
}
