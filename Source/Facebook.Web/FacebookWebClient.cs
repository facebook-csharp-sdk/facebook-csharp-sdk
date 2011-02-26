using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebRequest m_request;

        public FacebookWebClient()
            : this(FacebookWebRequest.Current)
        {
        }

        public FacebookWebClient(FacebookWebRequest request)
            : base(request.AccessToken)
        {
            this.m_request = request;
        }

        public override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            try
            {
                return base.Api(path, parameters, httpMethod);
            }
            catch (FacebookOAuthException)
            {
                this.m_request.DeleteAuthCookie();
                throw;
            }
        }

        public override object Api(string path, IDictionary<string, object> parameters, Type resultType, HttpMethod httpMethod)
        {
            try
            {
                return base.Api(path, parameters, resultType, httpMethod);
            }
            catch (FacebookOAuthException)
            {
                this.m_request.DeleteAuthCookie();
                throw;
            }
        }

        public override void ApiAsync<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback<T> callback, object state)
        {
            // TODO: Handle FacebookOAuthException
            base.ApiAsync<T>(path, parameters, httpMethod, callback, state);
        }

        public override void ApiAsync(FacebookAsyncCallback callback, object state, string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            // TODO: Handle FacebookOAuthException
            base.ApiAsync(callback, state, path, parameters, httpMethod);
        }

        public override void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, FacebookAsyncCallback callback, object state)
        {
            // TODO: Handle FacebookOAuthException
            base.ApiAsync(path, parameters, httpMethod, callback, state);
        }

    }
}
