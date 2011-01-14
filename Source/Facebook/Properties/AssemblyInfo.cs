// --------------------------------
// <copyright file="AssemblyInfo.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Facebook")]
#if !WINDOWS_PHONE
[assembly: InternalsVisibleTo("Facebook.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
#endif

[assembly: InternalsVisibleTo("Facebook.Web, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Web.Mvc, PublicKey=" + GlobalAssemblyInfo.PublicKey)]