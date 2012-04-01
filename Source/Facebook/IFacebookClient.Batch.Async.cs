namespace Facebook
{
    public partial interface IFacebookClient
    {
        void BatchAsync(FacebookBatchParameter[] batchParameters, object userState, object parameters);
        void BatchAsync(FacebookBatchParameter[] batchParameters, object userState);
        void BatchAsync(FacebookBatchParameter[] batchParameters);
    }
}
