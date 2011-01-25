
namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Represents the Facebook OAuth Helpers
    /// </summary>
    public class FacebookOAuthClientAuthorizer : IOAuthClientAuthorizer
    {
        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect uri.
        /// </summary>
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Gets the login uri.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the facebook login uri.
        /// </returns>
        /// <remarks>
        /// http://developers.facebook.com/docs/reference/dialogs/oauth
        /// Parameters that can be used:
        ///     client_id     : Your application's identifier. This is called client_id instead of app_id for this particular method to be compliant with the OAuth 2.0 specification. Required, but automatically specified by most SDKs.
        ///     redirect_uri  : The URL to redirect to after the user clicks a button on the dialog. Required, but automatically specified by most SDKs.
        ///     scope         : Optional. A comma-delimited list of permissions.
        ///     state         : Optional. An opaque string used to maintain application state between the request and callback. When Facebook redirects the user back to your redirect_uri, this value will be included unchanged in the response.
        ///     response_type : Optional, default is token. The requested response: an access token (token), an authorization code (code), or both (code_and_token).
        ///     display       : The display mode in which to render the dialog. The default is page on the www subdomain and wap on the m subdomain. This is automatically specified by most SDKs. (For WP7 builds it is set to touch.)
        /// </remarks>
        public Uri GetLoginUri(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            var defaultParameters = new Dictionary<string, object>();
            defaultParameters["client_id"] = this.ClientId;
            defaultParameters["redirect_uri"] = this.RedirectUri ?? new Uri("http://www.facebook.com/connect/login_success.html");
#if WINDOWS_PHONE
            defaultParameters["display"] = "touch";
#endif
            var mergedParameters = FacebookUtils.Merge(defaultParameters, parameters);

            // check if client_id and redirect_uri is not null or empty.
            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
            {
                throw new InvalidOperationException("client_id required.");
            }

            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
            {
                throw new InvalidOperationException("redirect_uri required.");
            }

            // seems like if we don't do this and rather pass the original uri object,
            // it seems to have http://localhost:80/csharpsamples instead of
            // http://localhost/csharpsamples
            // notice the port number, that shouldn't be there.
            // this seems to happen for iis hosted apps.
            mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();

            var url = "http://www.facebook.com/dialog/oauth/?" + FacebookUtils.ToJsonQueryString(mergedParameters);

            return new Uri(url);
        }

        // TODO: comment this for now.

        /*
        /// <summary>
        /// Gets the logout uri for desktop applications.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the desktop logout uri.
        /// </returns>
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
        */

#if !SILVERLIGHT // silverlight should have only async calls

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the access token or expires if exists.
        /// </returns>
        public object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters)
        {
            Contract.Requires(!string.IsNullOrEmpty(code));

            var pars = new Dictionary<string, object>();
            pars["client_id"] = this.ClientId;
            pars["client_secret"] = this.ClientSecret;
            pars["redirect_uri"] = this.RedirectUri;
            pars["code"] = code;

            var mergedParameters = FacebookUtils.Merge(pars, parameters);

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

            var queryString = FacebookUtils.ToJsonQueryString(mergedParameters);

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token") { Query = queryString.ToString() };

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

                var returnParameter = new JsonObject();
                FacebookApp.ParseQueryParametersToDictionary("?" + responseData, returnParameter);

                // access_token=string&expires=long or access_token=string
                // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
                var jsonObject = new Dictionary<string, object>();
                jsonObject["access_token"] = (string)returnParameter["access_token"];

                // check if expires exist coz for offline_access it is not present.
                if (returnParameter.ContainsKey("expires"))
                {
                    jsonObject.Add("expires", Convert.ToInt64(returnParameter["expires"]));
                }

                result = jsonObject;
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

#endif

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!string.IsNullOrEmpty(code));

            var pars = new Dictionary<string, object>();
            pars["client_id"] = this.ClientId;
            pars["client_secret"] = this.ClientSecret;
            pars["redirect_uri"] = this.RedirectUri;
            pars["code"] = code;

            var mergedParameters = FacebookUtils.Merge(pars, parameters);

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

            var queryString = FacebookUtils.ToJsonQueryString(mergedParameters);

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token");
            uriBuilder.Query = queryString;

            var requestUri = uriBuilder.Uri;
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "GET";

            request.BeginGetResponse(ar => this.ResponseCallback(ar, callback, state), request);
        }

        private void ResponseCallback(IAsyncResult asyncResult, FacebookAsyncCallback callback, object state)
        {
            object result = null;
            FacebookApiException exception = null;
            try
            {
                var responseData = string.Empty;
                var request = (HttpWebRequest)asyncResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asyncResult);

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }

                var returnParameter = new Dictionary<string, object>();
                FacebookApp.ParseQueryParametersToDictionary("?" + responseData, returnParameter);

                // access_token=string&expires=long or access_token=string
                // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
                var jsonObject = new JsonObject();
                jsonObject["access_token"] = (string)returnParameter["access_token"];

                // check if expires exist coz for offline_access it is not present.
                if (returnParameter.ContainsKey("expires"))
                {
                    jsonObject.Add("expires", Convert.ToInt64(returnParameter["expires"]));
                }

                result = jsonObject;
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
            finally
            {
                object data = null;
                if (exception == null)
                {
                    data = result;
                }

#if SILVERLIGHT
                callback(new FacebookAsyncResult(data, state, null, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));                
#else
                callback(new FacebookAsyncResult(data, state, asyncResult.AsyncWaitHandle, asyncResult.CompletedSynchronously, asyncResult.IsCompleted, exception));
#endif
            }
        }

        /// <summary>
        /// Gets the access token by exchanging the code.
        /// </summary>
        /// <param name="code">
        /// The code to exchange.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void ExchangeCodeForAccessTokenAsync(string code, IDictionary<string, object> parameters, FacebookAsyncCallback callback)
        {
            this.ExchangeCodeForAccessTokenAsync(code, parameters, callback, null);
        }
    }
}