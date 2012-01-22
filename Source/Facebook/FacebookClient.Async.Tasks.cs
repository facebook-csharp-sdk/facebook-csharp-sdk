namespace Facebook
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class FacebookClient
    {
        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
#if ASYNC_AWAIT
        /// <param name="uploadProgress">The upload progress</param>
#endif
        /// <returns>The task of json result with headers.</returns>
        public virtual Task<object> ApiTaskAsync(string httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken
#if ASYNC_AWAIT
, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
)
        {
            throw new NotImplementedException();
        }

#if ASYNC_AWAIT

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task of json result with headers.</returns>
        public virtual Task<object> ApiTaskAsync(string httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(httpMethod, path, parameters, resultType, userState, cancellationToken, null);
        }

#endif

    }
}