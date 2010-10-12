using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Facebook
{
#pragma warning disable 1591
    [ContractClassFor(typeof(FacebookAppBase))]
    public abstract class FacebookAppBaseContracts : FacebookAppBase
    {
        public override Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }
#if !SILVERLIGHT
        protected override void ValidateSessionObject(FacebookSession session)
        {
        }

        protected override string GenerateSignature(FacebookSession session)
        {
            Contract.Requires(session != null);
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return default(string);
        }

        protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(parameters.ContainsKey("method") && !String.IsNullOrEmpty((string)parameters["method"]));
            Contract.Ensures(Contract.Result<object>() != null);

            return default(object);
        }

        protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));
            Contract.Ensures(Contract.Result<object>() != null);

            return default(object);
        }

        protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(uri != null);
            Contract.Ensures(Contract.Result<object>() != null);

            return default(object);
        }
#endif
        protected override void RestServerAsync(FacebookAsyncCallback callback, object state, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(callback != null);
            Contract.Requires(parameters != null);
            Contract.Requires(parameters.ContainsKey("method") && !String.IsNullOrEmpty((string)parameters["method"]));
        }

        protected override void GraphAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(callback != null);
            Contract.Requires(!(String.IsNullOrEmpty(path) && parameters == null));
        }

        protected override void OAuthRequestAsync(FacebookAsyncCallback callback, object state, Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            Contract.Requires(callback != null);
            Contract.Requires(uri != null);
        }
    }
#pragma warning restore 1591
}
