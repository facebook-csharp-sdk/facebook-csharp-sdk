// --------------------------------
// <copyright file="FacebookAsyncResult.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the method that handles the post-call result.
    /// </summary>
    /// <param name="asyncResult">The Facebook asynchronous result.</param>
    [Obsolete]
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public delegate void FacebookAsyncCallback(FacebookAsyncResult asyncResult);

    /// <summary>
    /// Represents the method that handles the post-call result.
    /// </summary>
    /// <param name="asyncResult">The Facebook asynchronous result.</param>
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public delegate void FacebookAsyncCallback<T>(FacebookAsyncResult<T> asyncResult);

    /// <summary>
    /// Represents the status of an asynchronous Facebook api call.
    /// </summary>
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public class FacebookAsyncResult : IAsyncResult
    {
        /// <summary>
        /// The result of the asynchronous operation.
        /// </summary>
        private object result;

        /// <summary>
        /// The user-defined object that qualifies or contains information about an asynchronous operation
        /// </summary>
        private object asyncState;

        /// <summary>
        /// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        private System.Threading.WaitHandle asyncWaitHandle;

        /// <summary>
        /// A value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        private bool completedSynchronously;

        /// <summary>
        /// A value that indicates whether the asynchronous operation has completed.
        /// </summary>
        private bool isCompleted;

        /// <summary>
        /// The error that occurred processing this api call.
        /// </summary>
        private FacebookApiException error;

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
        /// Gets the result of an asynchronous operation.
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
        /// Gets a value indicating whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value></value>
        /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.</returns>
        public bool CompletedSynchronously
        {
            get { return this.completedSynchronously; }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation has completed.
        /// </summary>
        /// <value></value>
        /// <returns>true if the operation is complete; otherwise, false.</returns>
        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }
    }

    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
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
            get
            {
                if (base.Result == null)
                {
                    return default(T);
                }
                else
                {
                    return (T)base.Result;
                }
            }
        }

    }
}
