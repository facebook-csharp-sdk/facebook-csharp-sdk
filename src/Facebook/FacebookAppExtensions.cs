// --------------------------------
// <copyright file="FacebookAppExtensions.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Facebook
{
    /// <summary>
    /// Extension methods that add non-standard funcationality to the FacebookAppBase object.
    /// </summary>
    public static class FacebookAppExtensions
    {
#if (!SILVERLIGHT)
        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="query"></param>
        /// <returns></returns>
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
        /// <param name="app"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
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
            parameters["queries"] = JsonSerializer.SerializeObject(queryDict);
            parameters["method"] = "fql.multiquery";
            return app.Api(parameters); ;
        }
#endif
    }
}
