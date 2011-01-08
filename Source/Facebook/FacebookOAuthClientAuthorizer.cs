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
        #region Implementation of IOAuthClientAuthorizer

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

        // TODO: comment this for now. will need to support for GetLoginUri for web apps too
        // need to find a better name.

        /*
        /// <summary>
        /// Gets the login uri for desktop applications.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Returns the desktop login uri.
        /// </returns>
        public Uri GetDesktopLoginUri(IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(this.ClientId))
            {
                throw new Exception("ClientID required.");
            }

            var uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/authorize");

            var defaultParams = new Dictionary<string, object>();
            defaultParams["client_id"] = this.ClientId;
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

                // access_token=string&expires=long or access_token=string
                // Convert to JsonObject to support dynamic and be consistent with the rest of the library.
                var jsonObject = new JsonObject
                                     {
                                         { "access_token", returnParameter["access_token"] }
                                     };

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
                var jsonObject = new JsonObject
                                     {
                                         { "access_token", returnParameter["access_token"] }
                                     };

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

        #endregion
    }
}