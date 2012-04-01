
namespace Facebook
{
    public partial interface IFacebookClient
    {
        object Batch(params FacebookBatchParameter[] batchParameters);
        object Batch(FacebookBatchParameter[] batchParameters, object parameters);
    }
}
