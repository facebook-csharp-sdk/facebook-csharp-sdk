// --------------------------------
// <copyright file="FacebookBatchParameter.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents a batch parameter for the creating batch requests.
    /// </summary>
    /// <remarks>
    /// http://developers.facebook.com/docs/api/batch/
    /// </remarks>
    public class FacebookBatchParameter
    {
        /*
         * 
         * example of batch requesting showing usages of 
         * GET, POST query, multi-query and dependencies
         * 
         * 
           var fb = new FacebookClient("access_token");
           var result = fb.Batch(
                new FacebookBatchParameter { HttpMethod = HttpMethod.Get, Path = "/4" },
                new FacebookBatchParameter(HttpMethod.Get, "/me/friends", new Dictionary<string, object> { { "limit", 10 } }),
                new FacebookBatchParameter("/me/friends", new { limit = 1 }) { Data = new { name = "one-friend", omit_response_on_success = false } },
                new FacebookBatchParameter { Parameters = new { ids = "{result=one-friend:$.data.0.id}" } },
                new FacebookBatchParameter("{result=one-friend:$.data.0.id}/feed", new { limit = 5 }),
                new FacebookBatchParameter(HttpMethod.Post, "/me/feed", new { message = "test status update" }),
                new FacebookBatchParameter().Query("SELECT name FROM user WHERE uid=4"),
                new FacebookBatchParameter().Query(
                    "SELECT first_name FROM user WHERE uid=me()",
                    "SELECT last_name FROM user WHERE uid=me()"));
         * 
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        public FacebookBatchParameter()
        {
            HttpMethod = HttpMethod.Get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        public FacebookBatchParameter(string path)
            : this(HttpMethod.Get, path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        /// <param name="httpMethod">
        /// The http method.
        /// </param>
        /// <param name="path">
        /// The resource path.
        /// </param>
        public FacebookBatchParameter(HttpMethod httpMethod, string path)
            : this(httpMethod, path, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public FacebookBatchParameter(string path, object parameters)
            : this(HttpMethod.Get, path, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        /// <param name="httpMethod">
        /// The http method.
        /// </param>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public FacebookBatchParameter(HttpMethod httpMethod, string path, object parameters)
        {
            HttpMethod = httpMethod;
            Path = path;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the resource path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// Gets or sets the raw data parameter.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Data { get; set; }

        /// <summary>
        /// Returns a <see cref="FacebookBatchParameter"/> representing FQL query.
        /// </summary>
        /// <param name="fql">
        /// The fql query.
        /// </param>
        /// <returns>
        /// The <see cref="FacebookBatchParameter"/>.
        /// </returns>
        public FacebookBatchParameter Query(string fql)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException(fql);

            HttpMethod = HttpMethod.Get;
            Path = "fql";
            Parameters = new JsonObject { { "q", fql } };

            return this;
        }

        /// <summary>
        /// Returns a <see cref="FacebookBatchParameter"/> representing FQL multi-query.
        /// </summary>
        /// <param name="fql">
        /// The fql queries.
        /// </param>
        /// <returns>
        /// The <see cref="FacebookBatchParameter"/>.
        /// </returns>
        public FacebookBatchParameter Query(params string[] fql)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            var queryDict = new JsonObject();

            for (int i = 0; i < fql.Length; i++)
                queryDict.Add(string.Concat("query", i), fql[i]);

            HttpMethod = HttpMethod.Get;
            Path = "fql";
            Parameters = new JsonObject { { "q", queryDict } };

            return this;
        }
    }
}