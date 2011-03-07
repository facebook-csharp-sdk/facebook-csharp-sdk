// --------------------------------
// <copyright file="FacebookApiEventArgs.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents the facebook api event args.
    /// </summary>
    public class FacebookApiEventArgs : AsyncCompletedEventArgs
    {
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
        public FacebookApiEventArgs(Exception error, bool cancelled, object userState, string json)
            : base(error, cancelled, userState)
        {
            // check for error coz if its is rest api, json is not null
            if (error == null)
            {
                _json = json;
            }
        }

        /// <summary>
        /// Get the json result.
        /// </summary>
        /// <returns>
        /// The json result.
        /// </returns>
        public object GetResultData()
        {
            return JsonSerializer.Current.DeserializeObject(_json);
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
            return JsonSerializer.Current.DeserializeObject<T>(_json);
        }
    }
}
