// --------------------------------
// <copyright file="FacebookAsyncResult.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;

namespace Facebook
{
    /// <summary>
    /// Represents the method that handles the post-call result.
    /// </summary>
    /// <param name="ar"></param>
    public delegate void FacebookAsyncCallback(FacebookAsyncResult ar);

    /// <summary>
    /// Represents the status of an asynchronous Facebook api call.
    /// </summary>
    public class FacebookAsyncResult : IAsyncResult
    {
        object result;
        object asyncState;
        System.Threading.WaitHandle asyncWaitHandle;
        bool completedSynchronously;
        bool isCompleted;
        FacebookApiException error;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAsyncResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <param name="asyncWaitHandle">The async wait handle.</param>
        /// <param name="completedSynchronously">if set to <c>true</c> [completed synchronously].</param>
        /// <param name="isCompleted">if set to <c>true</c> [is completed].</param>
        /// <param name="error">The error.</param>
        public FacebookAsyncResult(
            object result,
            object asyncState,
            System.Threading.WaitHandle asyncWaitHandle,
            bool completedSynchronously,
            bool isCompleted,
            FacebookApiException error)
        {
            this.result = result;
            this.asyncState = asyncState;
            this.asyncWaitHandle = asyncWaitHandle;
            this.completedSynchronously = completedSynchronously;
            this.isCompleted = isCompleted;
            this.error = error;
        }

        /// <summary>
        /// Gets the error that occurred processing this api call.
        /// </summary>
        /// <value>The error.</value>
        public FacebookApiException Error
        {
            get { return this.error; }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public object Result
        {
            get { return this.result; }
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <value></value>
        /// <returns>A user-defined object that qualifies or contains information about an asynchronous operation.</returns>
        public object AsyncState
        {
            get { return this.asyncState; }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.</returns>
        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return this.asyncWaitHandle; }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value></value>
        /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
        public bool CompletedSynchronously
        {
            get { return this.completedSynchronously; }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        /// <value></value>
        /// <returns>true if the operation is complete; otherwise, false.</returns>
        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }
    }
}
