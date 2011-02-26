// --------------------------------
// <copyright file="JsonArray.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a json array.
    /// </summary>
    public sealed class JsonArray : List<object>
    {
        /// <summary>
        /// Json string representation of <see cref="JsonArray"/>.
        /// </summary>
        /// <returns>
        /// Returns the Json string representation of <see cref="JsonArray"/>.
        /// </returns>
        public override string ToString()
        {
            var result = JsonSerializer.Current.SerializeObject(this);
            return result ?? string.Empty;
        }

    }
}
