// --------------------------------
// <copyright file="FacebookClient.Sync.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>https://github.com/facebook-csharp-sdk/facbook-csharp-sdk</website>
// ---------------------------------

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