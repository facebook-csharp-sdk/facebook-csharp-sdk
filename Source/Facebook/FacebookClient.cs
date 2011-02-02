// --------------------------------
// <copyright file="FacebookApp.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    public class FacebookClient : FacebookClientBase
    {
        /// <summary>
        /// The multi-part form prefix characters.
        /// </summary>
        private const string Prefix = "--";

        /// <summary>
        /// The multi-part form new line characters.
        /// </summary>
        private const string NewLine = "\r\n";

        /// <summary>
        /// The collection of Facebook error types that should be retried.
        /// </summary>
        private static Collection<string> retryErrorTypes = new Collection<string>()
        {
            "OAuthException", // Graph OAuth Exception
            "190", // Rest OAuth Exception
        };

        /// <summary>
        /// How many times to retry a command if an error occurs until we give up.
        /// </summary>
        private int maxRetries = 2;

        /// <summary>
        /// How long in milliseconds to wait before retrying.
        /// </summary>
        private int retryDelay = 500;

        /// <summary>
        /// The current Facebook session.
        /// </summary>
        private FacebookSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        public FacebookClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="accessToken">The Facebook access token.</param>
        public FacebookClient(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));

            this.AccessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="appId">The Facebook application id.</param>
        /// <param name="appSecret">The Facebook application secret.</param>
        public FacebookClient(string appId, string appSecret)
        {
            Contract.Requires(!String.IsNullOrEmpty(appId));
            Contract.Requires(!String.IsNullOrEmpty(appSecret));

            this.AccessToken = String.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public FacebookClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                {
                    this.AccessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of times to retry an api
        /// call after experiencing a recoverable exception.
        /// </summary>
        /// <value>The max retries.</value>
        public int MaxRetries
        {
            get
            {
                return this.maxRetries;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.maxRetries = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in seconds to wait before retrying, with exponential roll off.
        /// </summary>
        /// <value>The retry delay.</value>
        public int RetryDelay
        {
            get
            {
                return this.retryDelay;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.retryDelay = value;
            }
        }

        /// <summary>
        /// Gets a collection of Facebook error types that
        /// should be retried in the event of a failure.
        /// </summary>
        protected virtual Collection<string> RetryErrorTypes
        {
            get { return retryErrorTypes; }
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public object Query(string fql)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            return this.Get(parameters);
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        public object Query(params string[] fql)
        {
            Contract.Requires(fql != null);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            return this.Get(parameters);
        }
#endif

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void QueryAsync(string fql, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(!String.IsNullOrEmpty(fql));

            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";
            this.GetAsync(parameters, callback, state);
        }

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void QueryAsync(string fql, FacebookAsyncCallback callback)
        {
            this.QueryAsync(fql, callback, null);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public void QueryAsync(string[] fql, FacebookAsyncCallback callback, object state)
        {
            Contract.Requires(fql != null);
            Contract.Requires(fql.Length > 0);

            var queryDict = new Dictionary<string, object>();
            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";
            this.GetAsync(parameters, callback, state);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        public void QueryAsync(string[] fql, FacebookAsyncCallback callback)
        {
            this.QueryAsync(fql, callback, null);
        }

#if !SILVERLIGHT

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="parameters">The parameters of the method call.</param>
        /// <param name="httpMethod">The http method for the request. Default is 'GET'.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override object RestServer(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            // Set the format to json
            parameters["format"] = "json-strings";
            Uri uri = this.GetApiUrl(parameters["method"].ToString());
            return this.OAuthRequest(uri, parameters, httpMethod, resultType, true);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>A dynamic object with the resulting data.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override object Graph(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            var uri = this.GetGraphRequestUri(path);

            return this.OAuthRequest(uri, parameters, httpMethod, resultType, false);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <returns>The decoded response object.</returns>
        protected override object OAuthRequest(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, this.AccessToken, out requestUrl, out contentType);

            return WithMirrorRetry<object>(() => { return MakeRequest(httpMethod, requestUrl, postData, contentType, resultType, restApi); });
        }

#endif

        /// <summary>
        /// Invoke the old restserver.php endpoint.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="parameters">The parameters of the method call.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override void RestServerAsync(IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
        {
            // Set the format to json
            parameters["format"] = "json-strings";
            Uri uri = this.GetApiUrl(parameters["method"].ToString());

            this.OAuthRequestAsync(uri, parameters, httpMethod, resultType, true, callback, state);
        }

        /// <summary>
        /// Make a graph api call.
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="path">The path of the url to call such as 'me/friends'.</param>
        /// <param name="parameters">JsonObject of url parameters.</param>
        /// <param name="httpMethod">The http method for the request.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override void GraphAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, FacebookAsyncCallback callback, object state)
        {
            var uri = this.GetGraphRequestUri(path);

            this.OAuthRequestAsync(uri, parameters, httpMethod, resultType, false, callback, state);
        }

        /// <summary>
        /// Make a OAuth Request
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="uri">The url to make the request.</param>
        /// <param name="parameters">The parameters of the request.</param>
        /// <param name="httpMethod">The http method of the request.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        protected override void OAuthRequestAsync(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
        {
            Uri requestUrl;
            string contentType;
            byte[] postData = BuildRequestData(uri, parameters, httpMethod, this.AccessToken, out requestUrl, out contentType);

            MakeRequestAsync(httpMethod, requestUrl, postData, contentType, resultType, restApi, callback, state);
        }

        /// <summary>
        /// This method invokes the supplied delegate with retry logic wrapped around it and returns the value of the delegate.
        /// If the delegate raises recoverable Facebook server or client errors, then the supplied delegate is reinvoked after
        /// a certain amount of delay until the retry limit is exceeded, at which point the exception is rethrown. Other
        /// exceptions are not caught and will be visible to callers.
        /// </summary>
        /// <typeparam name="TReturn">The type of object being returned</typeparam>
        /// <param name="body">The delegate to invoke within the retry logic which will produce the value to return</param>
        /// <returns>The value the delegate returns</returns>
        protected TReturn WithMirrorRetry<TReturn>(Func<TReturn> body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    return body();
                }
                catch (FacebookApiException ex)
                {
                    if (!this.RetryErrorTypes.Contains(ex.ErrorType))
                    {
                        throw;
                    }
                    else
                    {
                        if (retryCount >= this.maxRetries)
                        {
                            throw;
                        }
                    }
                }
                catch (WebException)
                {
                    if (retryCount >= this.maxRetries)
                    {
                        throw;
                    }
                }

                // Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(this.retryDelay);
                retryCount += 1;
            }
        }



        /// <summary>
        /// Builds the request post data and request uri based on the given parameters.
        /// </summary>
        /// <param name="uri">The request uri.</param>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="httpMethod">The http method.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="requestUrl">The outputted request uri.</param>
        /// <param name="contentType">The request content type.</param>
        /// <returns>The request post data.</returns>
        private static byte[] BuildRequestData(Uri uri, IDictionary<string, object> parameters, HttpMethod httpMethod, string accessToken, out Uri requestUrl, out string contentType)
        {
            Contract.Requires(uri != null);
            Contract.Requires(parameters != null);

            if (!parameters.ContainsKey("access_token") && !String.IsNullOrEmpty(accessToken))
            {
                parameters["access_token"] = accessToken;
            }

            var requestUrlBuilder = new UriBuilder(uri);

            // Set the default content type
            contentType = "application/x-www-form-urlencoded";
            byte[] postData = null;
            string queryString = string.Empty;

            if (httpMethod == HttpMethod.Get)
            {
                queryString = FacebookUtils.ToJsonQueryString(parameters);
            }
            else
            {
                queryString = string.Concat("access_token=", parameters["access_token"]);
                parameters.Remove("access_token");

                var containsMediaObject = parameters.Where(p => p.Value is FacebookMediaObject).Count() > 0;
                if (containsMediaObject)
                {
                    string boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);
                    postData = BuildMediaObjectPostData(parameters, boundary);
                    contentType = String.Concat("multipart/form-data; boundary=", boundary);
                }
                else
                {
                    postData = Encoding.UTF8.GetBytes(FacebookUtils.ToJsonQueryString(parameters));
                }
            }

            requestUrlBuilder.Query = queryString;
            requestUrl = requestUrlBuilder.Uri;

            return postData;
        }

        /// <summary>
        /// Builds the request post data if the request contains a media object
        /// such as an image or video to upload.
        /// </summary>
        /// <param name="parameters">The request parameters.</param>
        /// <param name="boundary">The multipart form request boundary.</param>
        /// <returns>The request post data.</returns>
        internal static byte[] BuildMediaObjectPostData(IDictionary<string, object> parameters, string boundary)
        {
            FacebookMediaObject mediaObject = null;

            // Build up the post message header
            var sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (kvp.Value is FacebookMediaObject)
                {
                    // Check to make sure the file upload hasn't already been set.
                    if (mediaObject != null)
                    {
                        throw ExceptionFactory.CannotIncludeMultipleMediaObjects;
                    }

                    mediaObject = kvp.Value as FacebookMediaObject;
                }
                else
                {
                    sb.Append(Prefix).Append(boundary).Append(NewLine);
                    sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                    sb.Append(NewLine);
                    sb.Append(NewLine);
                    sb.Append(kvp.Value);
                    sb.Append(NewLine);
                }
            }

            Debug.Assert(mediaObject != null, "The mediaObject is null.");

            if (mediaObject.ContentType == null || mediaObject.GetValue() == null || mediaObject.FileName == null)
            {
                throw ExceptionFactory.MediaObjectMustHavePropertiesSet;
            }

            sb.Append(Prefix).Append(boundary).Append(NewLine);
            sb.Append("Content-Disposition: form-data; filename=\"").Append(mediaObject.FileName).Append("\"").Append(NewLine);
            sb.Append("Content-Type: ").Append(mediaObject.ContentType).Append(NewLine).Append(NewLine);

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] fileData = mediaObject.GetValue();
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(String.Concat(NewLine, Prefix, boundary, Prefix, NewLine));

            // Combine all bytes to post
            var postData = new byte[postHeaderBytes.Length + fileData.Length + boundaryBytes.Length];
            Buffer.BlockCopy(postHeaderBytes, 0, postData, 0, postHeaderBytes.Length);
            Buffer.BlockCopy(fileData, 0, postData, postHeaderBytes.Length, fileData.Length);
            Buffer.BlockCopy(boundaryBytes, 0, postData, postHeaderBytes.Length + fileData.Length, boundaryBytes.Length);

            return postData;
        }

#if !SILVERLIGHT

        /// <summary>
        /// Make the API Request
        /// </summary>
        /// <param name="httpMethod">The http method to use. GET, POST, DELETE.</param>
        /// <param name="requestUrl">The uri of the request.</param>
        /// <param name="postData">The request data.</param>
        /// <param name="contentType">The request content type.</param>
        /// <returns>The decoded response object.</returns>
        /// <exception cref="Facebook.FacebookApiException" />
        private static object MakeRequest(HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType, Type resultType, bool restApi)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = FacebookUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.

            if (postData != null)
            {
                request.ContentLength = postData.Length;
                request.ContentType = contentType;
                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(postData, 0, postData.Length);
                }
            }

            object result = null;
            FacebookApiException exception = null;
            try
            {
                var responseData = String.Empty;
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }

                response.Close();

                // If we are using the REST API we need to check for an exception
                if (resultType == null || restApi)
                {
                    result = JsonSerializer.DeserializeObject(responseData);
                    if (restApi)
                    {
                        exception = ExceptionFactory.GetRestException(result);
                    }
                }

                if (exception != null)
                {
                    throw exception; // Thow the FacebookApiException
                }

                // Deserialize the final result if the result type is set
                if (resultType != null)
                {
                    result = JsonSerializer.DeserializeObject(responseData, resultType);
                }
            }
            catch (FacebookApiException)
            {
                // This is a rest api error thrown above, just rethrow
                throw;
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
        /// Make the API Request
        /// </summary>
        /// <param name="callback">The async callback.</param>
        /// <param name="state">The async state.</param>
        /// <param name="httpMethod">The http method to use. GET, POST, DELETE.</param>
        /// <param name="requestUrl">The uri of the request.</param>
        /// <param name="postData">The request data.</param>
        /// <param name="contentType">The request content type.</param>
        /// <exception cref="Facebook.FacebookApiException" />
        private static void MakeRequestAsync(HttpMethod httpMethod, Uri requestUrl, byte[] postData, string contentType, Type resultType, bool restApi, FacebookAsyncCallback callback, object state)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Method = FacebookUtils.ConvertToString(httpMethod); // Set the http method GET, POST, etc.
            if (httpMethod == HttpMethod.Post)
            {
                request.ContentType = contentType;
                request.BeginGetRequestStream((ar) => { RequestCallback(ar, postData, callback, state); }, request);
            }
            else
            {
                request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
            }
        }

        /// <summary>
        /// The asynchronous web request callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="callback">The callback method.</param>
        /// <param name="state">The asynchronous state.</param>
        private static void RequestCallback(IAsyncResult asyncResult, byte[] postData, FacebookAsyncCallback callback, object state)
        {
            var request = (HttpWebRequest)asyncResult.AsyncState;
            using (Stream stream = request.EndGetRequestStream(asyncResult))
            {
                stream.Write(postData, 0, postData.Length);
            }

            request.BeginGetResponse((ar) => { ResponseCallback(ar, callback, state); }, request);
        }

        /// <summary>
        /// The asynchronous web response callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="callback">The callback method.</param>
        /// <param name="state">The asynchronous state.</param>
        private static void ResponseCallback(IAsyncResult asyncResult, FacebookAsyncCallback callback, object state)
        {
            object result = null;
            FacebookApiException exception = null;
            try
            {
                var request = (HttpWebRequest)asyncResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asyncResult);

                using (Stream responseStream = response.GetResponseStream())
                {
                    result = JsonSerializer.DeserializeObject(responseStream);
                }
            }
            catch (FacebookApiException)
            {
                // Rest API Errors
                throw;
            }
            catch (WebException ex)
            {
                // Graph API Errors or general web exceptions
                exception = ExceptionFactory.GetGraphException(ex);
                if (exception != null)
                {
                    // Thow the FacebookApiException
                    throw exception;
                }

                throw;
            }
            finally
            {
                // Check to make sure there hasn't been an exception.
                // If there has, we want to pass null to the callback.
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
        /// Gets the graph request url in the proper format.
        /// </summary>
        /// <param name="path">The request url path.</param>
        /// <returns>The fully qualified uri for the request.</returns>
        private Uri GetGraphRequestUri(string path)
        {
            if (!String.IsNullOrEmpty(path) && path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1, path.Length - 1);
            }

            var uri = this.GetUrl("graph", path);
            return uri;
        }

        /// <summary>
        /// The code contracts invariant object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(this.maxRetries >= 0);
            Contract.Invariant(this.retryDelay >= 0);
            Contract.Invariant(retryErrorTypes != null);
        }
    }
}