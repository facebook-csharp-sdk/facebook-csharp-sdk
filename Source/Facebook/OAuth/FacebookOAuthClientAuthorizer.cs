namespace Facebook.OAuth
{
    using System;

    public class FacebookOAuthClientAuthorizer : IOAuthClientAuthorizer
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly Uri redirectUri;

        public FacebookOAuthClientAuthorizer()
        {
        }

        public FacebookOAuthClientAuthorizer(string clientId, string clientSecret, Uri redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
        }

        #region Implementation of IOAuthClientAuthorizer

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

        #endregion
    }
}