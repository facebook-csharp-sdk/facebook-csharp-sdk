// --------------------------------
// <copyright file="AssemblyInfo.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Facebook")]
[assembly: InternalsVisibleTo("Facebook.Web, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Web.Mvc, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Tests, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Api, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Api.Silverlight, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Api.Desktop, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("Facebook.Explorables, PublicKey=" + GlobalAssemblyInfo.PublicKey)]
