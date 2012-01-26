// --------------------------------
// <copyright file="FacebookApiEventArgs.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents the Facebook api event args.
    /// </summary>
    public class FacebookApiEventArgs : AsyncCompletedEventArgs
    {
        private readonly object _result;

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

        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, object result)
            : base(error, cancelled, userState)
        {
            _result = result;
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
            RaiseExceptionIfNecessary();
            return _result;
            //var json = JsonSerializer.Current.DeserializeObject(_json);

            //if (_isBatchResult)
            //{
            //    return FacebookClientOld.ProcessBatchResult(json);
            //}
            //if (_isQuery)
            //{
            //    // required for compatibility with v5.2.1
            //    var result = (IDictionary<string, object>)json;
            //    return result["data"];
            //}
            //return json;
        }
    }
}
