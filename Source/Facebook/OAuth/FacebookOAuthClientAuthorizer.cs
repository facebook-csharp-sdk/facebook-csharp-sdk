namespace Facebook.OAuth
{
    using System;
    using System.Collections.Generic;

    public class FacebookOAuthClientAuthorizer : IOAuthClientAuthorizer
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly Uri redirectUri;
        private readonly Uri authorizationServerUri;

        public FacebookOAuthClientAuthorizer()
        {
        }

        public FacebookOAuthClientAuthorizer(string clientId, string clientSecret, Uri redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
            this.authorizationServerUri = new Uri("https://graph.facebook.com/oauth/");
        }

        #region Implementation of IOAuthClientAuthorizer

        public Uri AuthorizationServerUri
        {
            get { return this.authorizationServerUri; }
        }

        public string ClientID
        {
            get { return this.clientId; }
        }

        public string ClientSecret
        {
            get { return this.clientSecret; }
        }

        public Uri RedirectUri
        {
            get { return this.redirectUri; }
        }

        public Uri GetDesktopLoginUri(IDictionary<string, object> parameters)
        {
            var uriBuilder = new UriBuilder(AuthorizationServerUri + "authorize");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["client_id"] = this.ClientID;
            defaultParams["redirect_uri"] = this.RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");

#if WINDOWS_PHONE
            defaultParams["display"] = "touch";
#else
            defaultParams["display"] = "popup";
#endif

            var mergedParameters = defaultParams.Merge(parameters);

            uriBuilder.Query = mergedParameters.ToJsonQueryString();

            return uriBuilder.Uri;
        }

        #endregion
    }
}