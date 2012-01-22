namespace Facebook
{
    using System;

    public partial class FacebookClient
    {
        public virtual object Batch(params FacebookBatchParameter[] batchParameters)
        {
            var parameters = PrepareBatchRequest(batchParameters);
            var result = Post(parameters);

            throw new NotImplementedException();
        }
    }
}