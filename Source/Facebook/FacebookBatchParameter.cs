// --------------------------------
// <copyright file="FacebookBatchParameter.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a batch parameter for the creating batch requests.
    /// </summary>
    /// <remarks>
    /// http://developers.facebook.com/docs/api/batch/
    /// </remarks>
    public class FacebookBatchParameter
    {
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
            Contract.Requires(!string.IsNullOrEmpty(fql));

            HttpMethod = HttpMethod.Get;
            Path = "/method/fql.query";
            Parameters = new Dictionary<string, object> { { "query", fql } };

            return this;
        }
    }
}