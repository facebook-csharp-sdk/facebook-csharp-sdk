// --------------------------------
// <copyright file="FacebookApiEventArgs.cs" company="Thuzi LLC (www.thuzi.com)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Represents the Facebook api event args.
    /// </summary>
    public class FacebookApiEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Indicates whether the result is a batch result.
        /// </summary>
        private readonly bool _isBatchResult;

        /// <summary>
        /// Indicates whether the result is a fql query.
        /// </summary>
        private readonly bool _isQuery;

        /// <summary>
        /// The json string.
        /// </summary>
        private readonly string _json;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiEventArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="cancelled">
        /// The cancelled.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        /// <param name="json">
        /// The json string.
        /// </param>
        /// <param name="isBatchResult">
        /// Indicates whether the result is a batch result.
        /// </param>
        /// <param name="isQuery">
        /// Indicates whether the result is a query
        /// </param>
        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json, bool isBatchResult, bool isQuery)
            : base(error, cancelled, userState)
        {
            _isBatchResult = isBatchResult;
            _isQuery = isQuery;

            // check for error coz if its is rest api, json is not null
            if (error == null)
                _json = json;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiEventArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="cancelled">
        /// The cancelled.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        /// <param name="json">
        /// The json string.
        /// </param>
        /// <param name="isBatchResult">
        /// Indicates whether the result is a batch result.
        /// </param>
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json, bool isBatchResult)
            : this(error, cancelled, userState, json, isBatchResult, false)
        {
        }

        /// <summary>
        /// Get the json result.
        /// </summary>
        /// <returns>
        /// The json result.
        /// </returns>
        public object GetResultData()
        {
            var json = JsonSerializer.Current.DeserializeObject(_json);

            if (_isBatchResult)
            {
                return FacebookClient.ProcessBatchResult(json);
            }
            if (_isQuery)
            {
                // required for compatibility with v5.2.1
                var result = (IDictionary<string, object>)json;
                return result["data"];
            }
            return json;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <typeparam name="T">
        /// Type of object to deserialize to.
        /// </typeparam>
        /// <returns>
        /// The result.
        /// </returns>
        public T GetResultData<T>()
        {
            if (_isBatchResult)
                throw new InvalidOperationException(FacebookUtils.Resources.GetResultDataGenericNotSupportedForBatchRequests);
            
            if (_isQuery && !string.IsNullOrEmpty(_json) && _json.StartsWith("{\"data\":") && _json.Length > 9)
            {
                // required for compatibility with v5.2.1
                var queryData = _json.Substring(8, _json.Length - 9);
                return JsonSerializer.Current.DeserializeObject<T>(queryData);
            }

            return JsonSerializer.Current.DeserializeObject<T>(_json);
        }
    }
}
