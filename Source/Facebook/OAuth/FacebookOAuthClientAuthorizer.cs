namespace Facebook.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    public class FacebookOAuthClientAuthorizer : IOAuthClientAuthorizer
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly Uri redirectUri;

        public FacebookOAuthClientAuthorizer()
            : this(null, null, null)
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

        public Uri GetDesktopLoginUri(IDictionary<string, object> parameters)
        {
            Contract.Requires(this.ClientID != null);

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/authorize");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["client_id"] = this.ClientID;
            defaultParams["redirect_uri"] = this.RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");

#if WINDOWS_PHONE
            defaultParams["display"] = "touch";
#elif CLIENTPROFILE || SILVERLIGHT
            defaultParams["display"] = "popup";
#else
            defaultParams["display"] = "page";
#endif

            var mergedParameters = defaultParams.Merge(parameters);

            uriBuilder.Query = mergedParameters.ToJsonQueryString();

            return uriBuilder.Uri;
        }

        public Uri GetDesktopLogoutUri(IDictionary<string, object> parameters)
        {
            // more information on this at http://stackoverflow.com/questions/2764436/facebook-oauth-logout
            var uriBuilder = new UriBuilder("http://m.facebook.com/logout.php");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["confirm"] = 1;
            defaultParams["next"] = this.RedirectUri ?? new Uri("http://www.facebook.com");

            var mergedParameters = defaultParams.Merge(parameters);

            uriBuilder.Query = mergedParameters.ToJsonQueryString();

            return uriBuilder.Uri;
        }

        public object ExchangeAccessTokenForCode(string code, IDictionary<string, object> parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(code));

            var pars = new Dictionary<string, object>();
            pars["client_id"] = this.ClientID;
            pars["client_secret"] = this.ClientSecret;
            pars["redirect_uri"] = this.RedirectUri;
            pars["code"] = code;

            var mergedParameters = pars.Merge(parameters);

            if (pars["client_id"] == null || string.IsNullOrEmpty(pars["client_id"].ToString()))
            {
                throw new Exception("ClientID required.");
            }

            if (pars["client_secret"] == null || string.IsNullOrEmpty(pars["client_secret"].ToString()))
            {
                throw new Exception("ClientSecret required");
            }

            if (pars["redirect_uri"] == null || string.IsNullOrEmpty(pars["redirect_uri"].ToString()))
            {
                throw new Exception("RedirectUri requried");
            }

            var queryString = mergedParameters.ToJsonQueryString();

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token");
            uriBuilder.Query = queryString;

            var requestUri = uriBuilder.Uri;
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "GET";

            object result;
            FacebookApiException exception = null;
            try
            {
                var responseData = string.Empty;
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }

                response.Close();

                var returnParameter = new Dictionary<string, object>();
                FacebookApp.ParseQueryParametersToDictionary("?" + responseData, returnParameter);
                result = returnParameter;
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    throw exception;
                }

                throw;
            }

            return result;
        }

        #endregion
    }
}