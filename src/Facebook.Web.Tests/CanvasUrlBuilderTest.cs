// <copyright file="CanvasUrlBuilderTest.cs" company="Thuzi, LLC">Microsoft Public License (Ms-PL)</copyright>
using System;
using System.Web;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Web
{
    /// <summary>This class contains parameterized unit tests for CanvasUrlBuilder</summary>
    [PexClass(typeof(CanvasUrlBuilder))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CanvasUrlBuilderTest
    {
        /// <summary>Test stub for BuildAuthCancelUrl()</summary>
        [PexMethod]
        public Uri BuildAuthCancelUrl([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            Uri result = target.BuildAuthCancelUrl();
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.BuildAuthCancelUrl(CanvasUrlBuilder)
        }

        /// <summary>Test stub for BuildAuthCancelUrl(String)</summary>
        [PexMethod]
        public Uri BuildAuthCancelUrl01(
            [PexAssumeUnderTest]CanvasUrlBuilder target,
            string pathAndQuery
        )
        {
            Uri result = target.BuildAuthCancelUrl(pathAndQuery);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.BuildAuthCancelUrl01(CanvasUrlBuilder, String)
        }

        /// <summary>Test stub for BuildAuthReturnUrl()</summary>
        [PexMethod]
        public Uri BuildAuthReturnUrl([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            Uri result = target.BuildAuthReturnUrl();
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.BuildAuthReturnUrl(CanvasUrlBuilder)
        }

        /// <summary>Test stub for BuildAuthReturnUrl(String)</summary>
        [PexMethod]
        public Uri BuildAuthReturnUrl01(
            [PexAssumeUnderTest]CanvasUrlBuilder target,
            string pathAndQuery
        )
        {
            Uri result = target.BuildAuthReturnUrl(pathAndQuery);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.BuildAuthReturnUrl01(CanvasUrlBuilder, String)
        }

        /// <summary>Test stub for BuildCanvasPageUrl(String)</summary>
        [PexMethod]
        public Uri BuildCanvasPageUrl(
            [PexAssumeUnderTest]CanvasUrlBuilder target,
            string pathAndQuery
        )
        {
            Uri result = target.BuildCanvasPageUrl(pathAndQuery);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.BuildCanvasPageUrl(CanvasUrlBuilder, String)
        }

        /// <summary>Test stub for get_CanvasPageApplicationPath()</summary>
        [PexMethod]
        public string CanvasPageApplicationPathGet([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            string result = target.CanvasPageApplicationPath;
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.CanvasPageApplicationPathGet(CanvasUrlBuilder)
        }

        /// <summary>Test stub for get_CanvasPageCurrentUrl()</summary>
        [PexMethod]
        public Uri CanvasPageCurrentUrlGet([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            Uri result = target.CanvasPageCurrentUrl;
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.CanvasPageCurrentUrlGet(CanvasUrlBuilder)
        }

        /// <summary>Test stub for get_CanvasUrl()</summary>
        [PexMethod]
        public Uri CanvasUrlGet([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            Uri result = target.CanvasUrl;
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.CanvasUrlGet(CanvasUrlBuilder)
        }

        /// <summary>Test stub for .ctor(HttpRequestBase)</summary>
        [PexMethod]
        public CanvasUrlBuilder Constructor(HttpRequestBase request)
        {
            CanvasUrlBuilder target = new CanvasUrlBuilder(request);
            return target;
            // TODO: add assertions to method CanvasUrlBuilderTest.Constructor(HttpRequestBase)
        }

        /// <summary>Test stub for get_CurrentCanvasPathAndQuery()</summary>
        [PexMethod]
        public string CurrentCanvasPathAndQueryGet([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            string result = target.CurrentCanvasPathAndQuery;
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.CurrentCanvasPathAndQueryGet(CanvasUrlBuilder)
        }

        /// <summary>Test stub for get_CurrentCanvasUrl()</summary>
        [PexMethod]
        public Uri CurrentCanvasUrlGet([PexAssumeUnderTest]CanvasUrlBuilder target)
        {
            Uri result = target.CurrentCanvasUrl;
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.CurrentCanvasUrlGet(CanvasUrlBuilder)
        }

        /// <summary>Test stub for GetCanvasRedirectHtml(Uri)</summary>
        [PexMethod]
        public string GetCanvasRedirectHtml(Uri url)
        {
            string result = CanvasUrlBuilder.GetCanvasRedirectHtml(url);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.GetCanvasRedirectHtml(Uri)
        }

        /// <summary>Test stub for GetCanvasRedirectHtml(String)</summary>
        [PexMethod]
        public string GetCanvasRedirectHtml01(string url)
        {
            string result = CanvasUrlBuilder.GetCanvasRedirectHtml(url);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.GetCanvasRedirectHtml01(String)
        }

        /// <summary>Test stub for GetLoginUrl(FacebookAppBase, String, String, String)</summary>
        [PexMethod]
        public Uri GetLoginUrl(
            [PexAssumeUnderTest]CanvasUrlBuilder target,
            FacebookAppBase facebookApp,
            string permissions,
            string returnUrlPath,
            string cancelUrlPath
        )
        {
            Uri result
               = target.GetLoginUrl(facebookApp, permissions, returnUrlPath, cancelUrlPath);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.GetLoginUrl(CanvasUrlBuilder, FacebookAppBase, String, String, String)
        }

        /// <summary>Test stub for GetLoginUrl(FacebookAppBase, String, String, String, Boolean)</summary>
        [PexMethod]
        public Uri GetLoginUrl01(
            [PexAssumeUnderTest]CanvasUrlBuilder target,
            FacebookAppBase facebookApp,
            string permissions,
            string returnUrlPath,
            string cancelUrlPath,
            bool cancelToSelf
        )
        {
            Uri result = target.GetLoginUrl
                             (facebookApp, permissions, returnUrlPath, cancelUrlPath, cancelToSelf);
            return result;
            // TODO: add assertions to method CanvasUrlBuilderTest.GetLoginUrl01(CanvasUrlBuilder, FacebookAppBase, String, String, String, Boolean)
        }
    }
}
