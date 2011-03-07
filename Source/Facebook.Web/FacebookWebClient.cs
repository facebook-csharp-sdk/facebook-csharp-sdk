namespace Facebook.Web
{
    using System;
    using System.Collections.Generic;

    public class FacebookWebClient : FacebookClient
    {
        private FacebookWebContext _request;

        public FacebookWebClient()
            : this(FacebookWebContext.Current)
        {
        }

        public FacebookWebClient(FacebookWebContext request)
            : base(request.AccessToken)
        {
            _request = request;
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
                    _request.DeleteAuthCookie();
                }
                catch { }
                throw;
            }
        }
    }
}
