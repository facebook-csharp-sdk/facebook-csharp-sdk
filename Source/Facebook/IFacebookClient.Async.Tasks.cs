namespace Facebook
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public partial interface IFacebookClient
    {
#if ASYNC_AWAIT
		Task<object> ApiTaskAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken);
#endif
		Task<object> GetTaskAsync(string path);
		Task<object> GetTaskAsync(object parameters);
		Task<object> GetTaskAsync(string path, object parameters);
		Task<object> GetTaskAsync(string path, object parameters, CancellationToken cancellationToken);
		Task<object> PostTaskAsync(object parameters);
		Task<object> PostTaskAsync(string path, object parameters);
		Task<object> PostTaskAsync(string path, object parameters, CancellationToken cancellationToken);
		Task<object> PostTaskAsync(string path, object parameters, object userState, CancellationToken cancellationToken);
#if ASYNC_AWAIT
		Task<object> PostTaskAsync(string path, object parameters, object userState, CancellationToken cancellationToken, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress);
#endif
		Task<object> DeleteTaskAsync(string path);
		Task<object> DeleteTaskAsync(string path, object parameters, CancellationToken cancellationToken);
    }
}
