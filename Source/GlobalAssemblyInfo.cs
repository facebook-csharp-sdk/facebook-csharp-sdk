// --------------------------------
// <copyright file="GlobalAssemblyInfo.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyDescription("Facebook C# SDK")]
[assembly: AssemblyCompany("Facebook C# SDK")]
[assembly: AssemblyProduct("Facebook C# SDK")]
[assembly: AssemblyCopyright("Microsoft Public License (Ms-PL)")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("5.0.2")]
[assembly: CLSCompliant(true)]

#if !(SILVERLIGHT || TESTS)
[assembly: AllowPartiallyTrustedCallers]
#endif

#if !WINDOWS_PHONE
[assembly: InternalsVisibleTo("Facebook.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
#endif

#if !(SILVERLIGHT || WINDOWS_PHONE)
    [assembly: InternalsVisibleTo("Facebook.Web, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
    [assembly: InternalsVisibleTo("Facebook.Web.Mvc, PublicKey=" + GlobalAssemblyInfo.PublicKey)]

    #if !TESTS
        [assembly: InternalsVisibleTo("Facebook.Web.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
        [assembly: InternalsVisibleTo("Facebook.Web.Mvc.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
    #endif

#endif

internal static class GlobalAssemblyInfo
{
    internal const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100c33f0459da2031" +
                                      "7bb7b839935988fff311e5f24c9719157a6552fc43d0c1f73f3f6af7b4d62aec8e30ae7639f0aa" +
                                      "942e3a7a61ad19acd83886efe42bb0c3453afb1b53e31dea327fb0e0f1c13578b53a402fc1193e" +
                                      "cde8acfbb125939d88a950f1bb4569ce67319b2c9774f6169354a64bcba0bb6f449ab3d8a7be56" +
                                      "e83ed9a6";
}