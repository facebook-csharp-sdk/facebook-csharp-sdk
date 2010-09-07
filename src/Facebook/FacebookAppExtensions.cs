// --------------------------------
// <copyright file="FacebookAppExtensions.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Diagnostics.Contracts;
using System.Dynamic;
using Facebook.Utilities;
using System.Collections.Generic;
using Facebook;
using System.Diagnostics;
using System.Globalization;

namespace Facebook
{
    public static class FacebookAppExtensions
    {
#if (!SILVERLIGHT)
        public static object Api(this FacebookAppBase app, string id, string path)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            Contract.EndContractBlock();

            return app.Api(BuildPath(id, path));
        }

        public static object Api(this FacebookAppBase app, string id, string path, HttpMethod httpMethod)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            Contract.EndContractBlock();

            return app.Api(BuildPath(id, path), httpMethod);
        }

        public static object Api(this FacebookAppBase app, string id, string path, IDictionary<string, object> parameters)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            Contract.EndContractBlock();

            return app.Api(BuildPath(id, path), parameters);
        }

        public static object Api(this FacebookAppBase app, string id, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            Contract.EndContractBlock();

            return app.Api(BuildPath(id, path), parameters, httpMethod);
        }

        private static string BuildPath(string id, string path)
        {
            if (id.StartsWith("/", StringComparison.Ordinal))
            {
                id = id.Substring(1);
            }
            if (id.EndsWith("/", StringComparison.Ordinal))
            {
                id = id.Substring(0, id.Length - 1);
            }
            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return String.Format(CultureInfo.InvariantCulture, "/{0}/{1}", id, path);
        }

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static object Fql(this FacebookAppBase app, string query)
        {
            // <pex>
            if (app == (FacebookAppBase)null)
                throw new ArgumentNullException("app");
            // </pex>
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }

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
            // <pex>
            if (app == (FacebookAppBase)null)
                throw new ArgumentNullException("app");
            // </pex>
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

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
