// <copyright file="CanvasSettingsTest.cs" company="Thuzi, LLC">Microsoft Public License (Ms-PL)</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Web
{
    /// <summary>This class contains parameterized unit tests for CanvasSettings</summary>
    [PexClass(typeof(CanvasSettings))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class CanvasSettingsTest
    {
        /// <summary>Test stub for get_Current()</summary>
        [PexMethod]
        public ICanvasSettings CurrentGet()
        {
            ICanvasSettings result = CanvasSettings.Current;
            return result;
            // TODO: add assertions to method CanvasSettingsTest.CurrentGet()
        }
    }
}
