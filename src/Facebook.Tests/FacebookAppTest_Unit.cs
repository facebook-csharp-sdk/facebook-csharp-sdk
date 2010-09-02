using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests
{
    [TestClass]
    public partial class FacebookAppTest
    {

        /// <summary>
        ///A test for BuildMediaObjectPostData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Facebook.dll")]
        public void BuildMediaObjectPostDataTest()
        {
            dynamic parameters = null; // TODO: Initialize to an appropriate value
            string boundary = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = FacebookApp_Accessor.BuildMediaObjectPostData(parameters, boundary);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
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
