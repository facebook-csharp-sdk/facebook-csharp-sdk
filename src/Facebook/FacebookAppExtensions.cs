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
namespace Facebook
{
    public static class FacebookAppExtensions
    {
#if (!SILVERLIGHT)
        public static dynamic Api(this FacebookAppBase app, string id, string path)
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

        public static dynamic Api(this FacebookAppBase app, string id, string path, HttpMethod httpMethod)
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

        public static dynamic Api(this FacebookAppBase app, string id, string path, dynamic parameters)
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

        public static dynamic Api(this FacebookAppBase app, string id, string path, dynamic parameters, HttpMethod httpMethod)
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
            if (id.StartsWith("/"))
            {
                id = id.Substring(1);
            }
            if (id.EndsWith("/"))
            {
                id = id.Substring(0, id.Length - 1);
            }
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, id.Length - 1);
            }
            return string.Format("/{0}/{1}", id, path);
        }

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static dynamic Fql(this FacebookApp app, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }

            dynamic parameters = new ExpandoObject();
            parameters.query = query;
            parameters.method = "fql.query";
            dynamic result = app.Api(parameters);
            return result;
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        public static dynamic Fql(this FacebookApp app, params string[] queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

            dynamic queryObj = new ExpandoObject();
            IDictionary<string, object> queryDict = (IDictionary<string, object>)queryObj;
            for (int i = 0; i < queries.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), queries[i]);
            }
            dynamic parameters = new ExpandoObject();
            parameters.queries = JsonSerializer.SerializeObject(queryObj);
            parameters.method = "fql.multiquery";
            dynamic result = app.Api(parameters);
            return result;
        }
#endif
    }
}
