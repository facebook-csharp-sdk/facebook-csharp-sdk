using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    public class FacebookWebClient : FacebookClient
    {
        private FacebookHttpRequest m_request;

        public FacebookWebClient()
            : this(FacebookHttpRequest.Current)
        {
        }

        public FacebookWebClient(FacebookHttpRequest request)
            : base(request.AccessToken)
        {
            this.m_request = request;
        }

        protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            try
            {
                return base.Api(path, parameters, httpMethod, resultType);
            }
            catch (FacebookOAuthException)
            {
                try
                {
                    this.m_request.DeleteAuthCookie();
                }
                catch { }
                throw;
            }
        }
    }
}
