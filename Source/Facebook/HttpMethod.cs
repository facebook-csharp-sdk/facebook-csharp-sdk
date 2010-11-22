// --------------------------------
// <copyright file="HttpMethod.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------


namespace Facebook
{
    /// <summary>
    /// Represents an HTTP request type.
    /// </summary>
    internal enum HttpMethod
    {
        /// <summary>
        /// A GET Request
        /// </summary>
        Get,
        /// <summary>
        /// A POST Request
        /// </summary>
        Post,
        /// <summary>
        /// A DELETE Request
        /// </summary>
        Delete,
    }
}
