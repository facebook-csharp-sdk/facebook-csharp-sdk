// --------------------------------
// <copyright file="FacebookAppExtensions.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension methods that add non-standard funcationality to the FacebookAppBase object.
    /// </summary>
    [Obsolete("Use Facebook.Web.FacebookWebClient instead.")]
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public static class FacebookAppExtensions
    {
#if (!SILVERLIGHT)
        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="fql">The FQL query.</param>
        /// <returns>The FQL query result.</returns>
        public static object Query(this FacebookAppBase app, string fql)
        {
            Contract.Requires(app != null);
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            return app.Get(parameters);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="fql">The FQL queries.</param>
        /// <returns>A collection of the FQL query results.</returns>
        public static object Query(this FacebookAppBase app, params string[] fql)
        {
            Contract.Requires(app != null);
            Contract.Requires(fql != null);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            return app.Get(parameters);
        }

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="query">The FQL query.</param>
        /// <returns>The FQL query result.</returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("You should use Query rather than this method. This method will be removed in the next version.")]
        public static object Fql(this FacebookAppBase app, string query)
        {
            Contract.Requires(app != null);
            Contract.Requires(!String.IsNullOrEmpty(query));

            return app.Query(query);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="queries">The FQL queries.</param>
        /// <returns>A collection of the FQL query results.</returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("You should use Query rather than this method. This method will be removed in the next version.")]
        public static object Fql(this FacebookAppBase app, params string[] queries)
        {
            Contract.Requires(app != null);
            Contract.Requires(queries != null);

            return app.Query(queries);
        }
#endif
    }
}
