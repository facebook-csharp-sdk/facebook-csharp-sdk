using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebContext m_request;

        public FacebookWebClient()
            : this(FacebookWebContext.Current)
        {
        }

        public FacebookWebClient(FacebookWebContext request)
            : base(request.AccessToken)
        {
            this.m_request = request;
        }

        internal protected override object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
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
