// <copyright file="CanvasConfigurationSettingsTest.cs" company="Thuzi, LLC">Microsoft Public License (Ms-PL)</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Web
{
    /// <summary>This class contains parameterized unit tests for CanvasConfigurationSettings</summary>
    [PexClass(typeof(CanvasConfigurationSettings))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CanvasConfigurationSettingsTest
    {
        /// <summary>Test stub for get_AuthorizeCancelUrl()</summary>
        [PexMethod]
        public Uri AuthorizeCancelUrlGet([PexAssumeUnderTest]CanvasConfigurationSettings target)
        {
            Uri result = target.AuthorizeCancelUrl;
            return result;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.AuthorizeCancelUrlGet(CanvasConfigurationSettings)
        }

        /// <summary>Test stub for set_AuthorizeCancelUrl(Uri)</summary>
        [PexMethod]
        public void AuthorizeCancelUrlSet(
            [PexAssumeUnderTest]CanvasConfigurationSettings target,
            Uri value
        )
        {
            target.AuthorizeCancelUrl = value;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.AuthorizeCancelUrlSet(CanvasConfigurationSettings, Uri)
        }

        /// <summary>Test stub for get_CanvasPageUrl()</summary>
        [PexMethod]
        public Uri CanvasPageUrlGet([PexAssumeUnderTest]CanvasConfigurationSettings target)
        {
            Uri result = target.CanvasPageUrl;
            return result;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.CanvasPageUrlGet(CanvasConfigurationSettings)
        }

        /// <summary>Test stub for set_CanvasPageUrl(Uri)</summary>
        [PexMethod]
        public void CanvasPageUrlSet(
            [PexAssumeUnderTest]CanvasConfigurationSettings target,
            Uri value
        )
        {
            target.CanvasPageUrl = value;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.CanvasPageUrlSet(CanvasConfigurationSettings, Uri)
        }

        /// <summary>Test stub for get_CanvasUrl()</summary>
        [PexMethod]
        public Uri CanvasUrlGet([PexAssumeUnderTest]CanvasConfigurationSettings target)
        {
            Uri result = target.CanvasUrl;
            return result;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.CanvasUrlGet(CanvasConfigurationSettings)
        }

        /// <summary>Test stub for set_CanvasUrl(Uri)</summary>
        [PexMethod]
        public void CanvasUrlSet(
            [PexAssumeUnderTest]CanvasConfigurationSettings target,
            Uri value
        )
        {
            target.CanvasUrl = value;
            // TODO: add assertions to method CanvasConfigurationSettingsTest.CanvasUrlSet(CanvasConfigurationSettings, Uri)
        }
    }
}
