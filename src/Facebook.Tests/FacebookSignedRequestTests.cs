// --------------------------------
// <copyright file="FacebookSignedRequestTests.cs" company="Thuzi, LLC">
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

namespace Facebook.Tests
{
    [TestClass]
    public class FacebookSignedRequestTests
    {

        [TestMethod]
        public void Test_Signed_Request()
        {
            var signed_request = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
            FacebookApp app = new FacebookApp();
            var signedRequest = app.ParseSignedRequest(signed_request);
            Assert.AreEqual("120625701301347|2.I3WPFn_9kJegQNDf5K_I2g__.3600.1282928400-14812017|qrfiOepbv4fswcdYtRWfANor9bQ.", signedRequest.AccessToken);
        }

    }
}
