// --------------------------------
// <copyright file="FqlExtensions.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebookgraphtoolkit.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Facebook.Utilities;

namespace Facebook.Api.Fql
{
    public static class FqlExtensions
    {

#if (!SILVERLIGHT)
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
