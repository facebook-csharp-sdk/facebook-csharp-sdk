// --------------------------------
// <copyright file="FacebookAsyncResult.cs" company="Thuzi LLC (www.thuzi.com)">
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
    /// Represents the method that handles the post-call result.
    /// </summary>
    /// <param name="asyncResult">The Facebook asynchronous result.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete]
    public delegate void FacebookAsyncCallback(FacebookAsyncResult asyncResult);

    /// <summary>
    /// Represents the method that handles the post-call result.
    /// </summary>
    /// <param name="asyncResult">The Facebook asynchronous result.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete]
    public delegate void FacebookAsyncCallback<T>(FacebookAsyncResult<T> asyncResult);

    /// <summary>
    /// Represents the status of an asynchronous Facebook api call.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete]
    public class FacebookAsyncResult : IAsyncResult
    {
        /// <summary>
        /// The result of the asynchronous operation.
        /// </summary>
        private readonly object _result;

        /// <summary>
        /// The user-defined object that qualifies or contains information about an asynchronous operation
        /// </summary>
        private readonly object _asyncState;

        /// <summary>
        /// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        private readonly System.Threading.WaitHandle _asyncWaitHandle;

        /// <summary>
        /// A value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        private readonly bool _completedSynchronously;

        /// <summary>
        /// A value that indicates whether the asynchronous operation has completed.
        /// </summary>
        private readonly bool _isCompleted;

        /// <summary>
        /// The error that occurred processing this api call.
        /// </summary>
        private readonly FacebookApiException _error;

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
            _result = result;
            _asyncState = asyncState;
            _asyncWaitHandle = asyncWaitHandle;
            _completedSynchronously = completedSynchronously;
            _isCompleted = isCompleted;
            _error = error;
        }

        /// <summary>
        /// Gets the error that occurred processing this api call.
        /// </summary>
        /// <value>The error.</value>
        public FacebookApiException Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Gets the result of an asynchronous operation.
        /// </summary>
        /// <value>The result.</value>
        public object Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <value></value>
        /// <returns>A user-defined object that qualifies or contains information about an asynchronous operation.</returns>
        public object AsyncState
        {
            get { return _asyncState; }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.</returns>
        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return _asyncWaitHandle; }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value></value>
        /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation has completed.
        /// </summary>
        /// <value></value>
        /// <returns>true if the operation is complete; otherwise, false.</returns>
        public bool IsCompleted
        {
            get { return _isCompleted; }
        }
    }

    /// <summary>
    /// Represents the status of a generic asynchronous Facebook api call.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete]
    public class FacebookAsyncResult<T> : FacebookAsyncResult
    {
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
            : base(result, asyncState, asyncWaitHandle, completedSynchronously, isCompleted, error)
        {
        }

        /// <summary>
        /// Gets the result of an asynchronous operation.
        /// </summary>
        /// <value>The result.</value>
        public new T Result
        {
            get { return base.Result == null ? default(T) : (T) base.Result; }
        }

    }
}
