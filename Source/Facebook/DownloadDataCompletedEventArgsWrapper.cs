// --------------------------------
// <copyright file="DownloadDataCompletedEventArgsWrapper.cs" company="Facebook C# SDK">
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
    /// Represents a wrapper for DownloadDataCompletedEventArgs
    /// </summary>
    internal class DownloadDataCompletedEventArgsWrapper : AsyncCompletedEventArgs
    {
        /// <summary>
        /// The async result.
        /// </summary>
        private readonly byte[] _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadDataCompletedEventArgsWrapper"/> class.
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
        /// <param name="result">
        /// The result.
        /// </param>
        public DownloadDataCompletedEventArgsWrapper(Exception error, bool cancelled, object userState, byte[] result)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        /// <summary>
        /// Gets the async result.
        /// </summary>
        public byte[] Result
        {
            get { return _result; }
        }
    }
}