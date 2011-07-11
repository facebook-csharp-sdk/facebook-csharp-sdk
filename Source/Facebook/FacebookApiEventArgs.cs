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
        [Obsolete]
        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json)
            : this(error, cancelled, userState, json, false)
        {
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
        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json, bool isBatchResult)
            : base(error, cancelled, userState)
        {
            _isBatchResult = isBatchResult;

            // check for error coz if its is rest api, json is not null
            if (error == null)
                _json = json;
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

            return _isBatchResult ? FacebookClient.ProcessBatchResult(json) : json;
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
                throw new InvalidOperationException(Properties.Resources.GetResultDataGenericNotSupportedForBatchRequests);

            return JsonSerializer.Current.DeserializeObject<T>(_json);
        }
    }
}
