// <copyright file="FacebookAppRedirectHttpHandlerTest.cs" company="Thuzi, LLC">Microsoft Public License (Ms-PL)</copyright>
using System;
using System.Web;
using Facebook.Web;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Web
{
    /// <summary>This class contains parameterized unit tests for FacebookAppRedirectHttpHandler</summary>
    [PexClass(typeof(FacebookAppRedirectHttpHandler))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class FacebookAppRedirectHttpHandlerTest
    {
        /// <summary>Test stub for get_IsReusable()</summary>
        [PexMethod]
        public bool IsReusableGet([PexAssumeUnderTest]FacebookAppRedirectHttpHandler target)
        {
            bool result = target.IsReusable;
            return result;
            // TODO: add assertions to method FacebookAppRedirectHttpHandlerTest.IsReusableGet(FacebookAppRedirectHttpHandler)
        }

        /// <summary>Test stub for ProcessRequest(HttpContext)</summary>
        [PexMethod]
        public void ProcessRequest(
            [PexAssumeUnderTest]FacebookAppRedirectHttpHandler target,
            HttpContext context
        )
        {
            target.ProcessRequest(context);
            // TODO: add assertions to method FacebookAppRedirectHttpHandlerTest.ProcessRequest(FacebookAppRedirectHttpHandler, HttpContext)
        }
    }
}
