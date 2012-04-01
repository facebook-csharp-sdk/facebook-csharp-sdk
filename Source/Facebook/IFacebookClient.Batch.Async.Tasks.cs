namespace Facebook
{
    using System.Threading;
    using System.Threading.Tasks;

    public partial interface IFacebookClient
    {
        Task<object> BatchTaskAsync(params FacebookBatchParameter[] batchParameters);
        Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userState, CancellationToken cancellationToken
#if ASYNC_AWAIT
, System.IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
);
        Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userState, object parameters, CancellationToken cancellationToken
#if ASYNC_AWAIT
, System.IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
);
#if ASYNC_AWAIT
		Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, CancellationToken cancellationToken);
		Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, object parameters, CancellationToken cancellationToken);
#endif
    }
}
