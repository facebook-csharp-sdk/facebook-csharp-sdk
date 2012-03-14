﻿//-----------------------------------------------------------------------
// <copyright file="FacebookClient.Batch.Async.Tasks.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
//-----------------------------------------------------------------------

namespace Facebook
{
    using System.Threading;
    using System.Threading.Tasks;

    public partial class FacebookClient
    {
        public virtual Task<object> BatchTaskAsync(params FacebookBatchParameter[] batchParameters)
        {
            return BatchTaskAsync(batchParameters, null, CancellationToken.None);
        }

        public virtual Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, CancellationToken cancellationToken
#if ASYNC_AWAIT
, System.IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
)
        {
            return BatchTaskAsync(batchParameters, userToken, null, cancellationToken
#if ASYNC_AWAIT
, uploadProgress
#endif
                );
        }

        public virtual Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, object parameters, CancellationToken cancellationToken
#if ASYNC_AWAIT
, System.IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress
#endif
)
        {
            var actualParameter = PrepareBatchRequest(batchParameters, parameters);
            return PostTaskAsync(null, actualParameter, userToken, cancellationToken
#if ASYNC_AWAIT
, uploadProgress
#endif
            );
        }

#if ASYNC_AWAIT

        public virtual Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, CancellationToken cancellationToken)
        {
            return BatchTaskAsync(batchParameters, userToken, null, cancellationToken);
        }

        public virtual Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, object parameters, CancellationToken cancellationToken)
        {
            return BatchTaskAsync(batchParameters, userToken, parameters, cancellationToken, null);
        }

#endif

    }
}