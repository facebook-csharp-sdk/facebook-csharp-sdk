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

    /// <summary>
    /// Extension methods that add non-standard funcationality to the FacebookAppBase object.
    /// </summary>
    public static class FacebookAppExtensions
    {
#if (!SILVERLIGHT)
        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="query">The FQL query.</param>
        /// <returns>The FQL query result.</returns>
        public static object Fql(this FacebookAppBase app, string query)
        {
            Contract.Requires(app != null);
            Contract.Requires(!String.IsNullOrEmpty(query));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = query;
            parameters["method"] = "fql.query";
            return app.Api(parameters);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="app">The Facebook app.</param>
        /// <param name="queries">The FQL queries.</param>
        /// <returns>A collection of the FQL query results.</returns>
        public static object Fql(this FacebookAppBase app, params string[] queries)
        {
            Contract.Requires(app != null);
            Contract.Requires(queries != null);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < queries.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), queries[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            return app.Api(parameters);
        }
#endif
    }
}
