// --------------------------------
// <copyright file="FacebookBatchParameter.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

namespace Facebook
{
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
         * TODO: UPDATE SAMPLE
         * 
         * example of batch requesting showing usages of 
         * GET, POST query, multi-query and dependencies
         * 
         * 
           var fb = new FacebookClient("access_token");
           var result = fb.Batch(
                new FacebookBatchParameter { HttpMethod = "GET", Path = "/4" },
                new FacebookBatchParameter("GET", "/me/friends", new Dictionary<string, object> { { "limit", 10 } }),
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
            HttpMethod = "GET";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookBatchParameter"/> class.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        public FacebookBatchParameter(string path)
            : this("GET", path)
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
        public FacebookBatchParameter(string httpMethod, string path)
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
            : this("GET", path, parameters)
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
        public FacebookBatchParameter(string httpMethod, string path, object parameters)
        {
            HttpMethod = httpMethod;
            Path = path;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public string HttpMethod { get; set; }

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
    }
}