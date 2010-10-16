// <copyright file="GlobalAssemblyInfoTest.cs" company="Thuzi, LLC">Microsoft Public License (Ms-PL)</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>This class contains parameterized unit tests for GlobalAssemblyInfo</summary>
[PexClass(typeof(global::GlobalAssemblyInfo))]
[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
[PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
[TestClass]
public partial class GlobalAssemblyInfoTest
{
}
