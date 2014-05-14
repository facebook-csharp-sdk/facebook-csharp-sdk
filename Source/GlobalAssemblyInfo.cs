//-----------------------------------------------------------------------
// <copyright file="<file>.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Reflection;
using System.Runtime.InteropServices;

#if !TESTS
[assembly: CLSCompliant(true)]
#endif


#if !(SILVERLIGHT || NETFX_CORE || TESTS)
[assembly: AllowPartiallyTrustedCallers]
    #if !NET35
        [assembly: SecurityRules(SecurityRuleSet.Level1)]
    #endif
#endif

#if !(SILVERLIGHT || WINDOWS_PHONE)
[assembly: InternalsVisibleTo("Facebook.Web, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Web.Mvc, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Web.Compatibility, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
#if !TESTS
[assembly: InternalsVisibleTo("Facebook.Web.Tests,PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Web.Mvc.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
#endif

#endif

#if SILVERLIGHT
[assembly: InternalsVisibleTo("Facebook.Tests")]
#endif

// Expose Internals to Client SDKs for WinRT and Windows Phone
#if WINDOWS_PHONE
[assembly: InternalsVisibleTo("Facebook.Client")]
#endif
#if NETFX_CORE
[assembly: InternalsVisibleTo("Facebook.Client, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
#endif


internal static class GlobalAssemblyInfo
{
    internal const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100c33f0459da2031" +
                                      "7bb7b839935988fff311e5f24c9719157a6552fc43d0c1f73f3f6af7b4d62aec8e30ae7639f0aa" +
                                      "942e3a7a61ad19acd83886efe42bb0c3453afb1b53e31dea327fb0e0f1c13578b53a402fc1193e" +
                                      "cde8acfbb125939d88a950f1bb4569ce67319b2c9774f6169354a64bcba0bb6f449ab3d8a7be56" +
                                      "e83ed9a6";
}