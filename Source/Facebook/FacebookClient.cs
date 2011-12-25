// --------------------------------
// <copyright file="FacebookClient.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using FluentHttp;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    public class FacebookClient
    {
        /// <summary>
        /// The value indicating whether to use Facebook beta.
        /// </summary>
        private bool _useFacebookBeta = FacebookApplication.Current.UseFacebookBeta;

        /// <summary>
        /// Gets a value indicating whether the scheme is secure.
        /// </summary>
        private bool _isSecureConnection = FacebookApplication.Current.IsSecureConnection;

        /// <summary>
        /// The Facebook access token.
        /// </summary>
        private string _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class. 
        /// </summary>
        public FacebookClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class. 
        /// </summary>
        /// <param name="accessToken">
        /// The Facebook access token.
        /// </param>
        public FacebookClient(string accessToken)
            : this()
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");
            _accessToken = accessToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public FacebookClient(string appId, string appSecret)
            : this(string.Concat(appId, "|", appSecret))
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");
            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException("appSecret");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The Facebook application.
        /// </param>
        public FacebookClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                {
                    _accessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);
                }
            }
        }

        /// <summary>
        /// Event handler for get completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> GetCompleted;

        /// <summary>
        /// Event handler for post completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> PostCompleted;

        /// <summary>
        /// Event handler for delete completion.
        /// </summary>
        public event EventHandler<FacebookApiEventArgs> DeleteCompleted;

        /// <summary>
        /// Event handler for upload progress changed.
        /// </summary>
        public event EventHandler<FacebookUploadProgressChangedEventArgs> UploadProgressChanged;

#if TPL

        /// <summary>
        /// Event handler when http web request wrapper is created for async api only.
        /// (used internally by TPL for cancellation support)
        /// </summary>
        private event EventHandler<HttpWebRequestCreatedEventArgs> HttpWebRequestWrapperCreated;

#endif

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public virtual string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use Facebook beta.
        /// </summary>
        public virtual bool UseFacebookBeta
        {
            get { return _useFacebookBeta; }
            set { _useFacebookBeta = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scheme is secure.
        /// </summary>
        public virtual bool IsSecureConnection
        {
            get { return _isSecureConnection; }
            set { _isSecureConnection = value; }
        }

        /// <summary>
        /// Gets the aliases to Facebook domains.
        /// </summary>
        protected virtual Dictionary<string, Uri> DomainMaps
        {
            get { return UseFacebookBeta ? FacebookUtils.DomainMapsBeta : FacebookUtils.DomainMaps; }
        }

        #region Api Calls

#if !(SILVERLIGHT || WINRT)

        #region Get

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        public virtual object Get(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return Api(path, null, HttpMethod.Get, null);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Get(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            return Api(null, parameters, HttpMethod.Get, null);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Get(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return Api(path, parameters, HttpMethod.Get, null);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual T Get<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return Api<T>(path, null, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual T Get<T>(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return Api<T>(path, parameters, HttpMethod.Get);
        }

        /// <summary>
        /// Makes a GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">
        /// The result of the API call.
        /// </typeparam>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual T Get<T>(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            return Api<T>(null, parameters, HttpMethod.Get);
        }

        #endregion

        #region Post

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Post(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            return Api(null, parameters, HttpMethod.Post, null);
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Post(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return Api(path, parameters, HttpMethod.Post, null);
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Post(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (parameters is IDictionary<string, object>)
                return Post((IDictionary<string, object>)parameters);

            if (parameters is string)
                return Post((string)parameters, null);

            return Post(FacebookUtils.ToDictionary(parameters));
        }

        /// <summary>
        /// Makes a POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Post(string path, object parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            if (parameters is IDictionary<string, object>)
                return Post(path, (IDictionary<string, object>)parameters);

            return Post(path, FacebookUtils.ToDictionary(parameters));
        }

        #endregion

        #region Delete

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual object Delete(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return Api(path, null, HttpMethod.Delete, null);
        }

        /// <summary>
        /// Makes a DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual object Delete(string path, IDictionary<string, object> parameters)
        {
            return Api(path, parameters, HttpMethod.Delete, null);
        }

        #endregion

        internal protected virtual object Api(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, Type resultType)
        {
            Stream input;

            IDictionary<string, FacebookMediaObject> mediaObjects;
            var httpHelper = new HttpHelper(PrepareRequest(path, parameters, httpMethod, out input, out mediaObjects));

            if (input != null)
            {
                // we have a request body, so write it.
                try
                {
                    using (Stream requestStream = httpHelper.OpenWrite())
                    {
                        FacebookUtils.CopyStream(input, requestStream, null);
                    }
                }
                catch (WebExceptionWrapper ex)
                {
                    if (ex.GetResponse() == null)
                        throw;
                }
                finally
                {
                    DisposeInputRequestStream(input);
                }
            }

            Stream responseStream;

            try
            {
                // responseStream is disposed by ProcessReponse method
                responseStream = httpHelper.OpenRead();
            }
            catch (WebExceptionWrapper ex)
            {
                if (ex.GetResponse() == null)
                    throw;

                responseStream = httpHelper.OpenRead();
            }

            Exception exception;
            string responseString;
            bool cancelled;

            var result = ProcessResponse(httpHelper, responseStream, resultType, out responseString, out exception, out cancelled);

            // unlike async versions, we don't need to check responseStream.CanRead
            // as we don't allow synchronous methods to be cancelled.
            if (exception == null)
                return result;

            throw exception;

        }

        protected T Api<T>(string path, IDictionary<string, object> parameters, HttpMethod httpMethod)
        {
            return (T)Api(path, parameters, httpMethod, typeof(T));
        }

#endif

        #region Async Api Calls

        #region Get

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        public virtual void GetAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            GetAsync(path, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void GetAsync(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            GetAsync(path, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void GetAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            ApiAsync(path, parameters, HttpMethod.Get, userToken);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void GetAsync(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            GetAsync(null, parameters);
        }

        #endregion

        #region Post

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void PostAsync(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            PostAsync(path, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void PostAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            ApiAsync(path, parameters, HttpMethod.Post, userToken);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void PostAsync(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            PostAsync(null, parameters);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void PostAsync(string path, object parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            if (parameters is IDictionary<string, object>)
            {
                PostAsync(path, (IDictionary<string, object>)parameters);
            }
            else
            {
                PostAsync(path, FacebookUtils.ToDictionary(parameters));
            }
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void PostAsync(string path, object parameters, object userToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            if (parameters is IDictionary<string, object>)
            {
                PostAsync(path, (IDictionary<string, object>)parameters, userToken);
            }
            else
            {
                PostAsync(path, FacebookUtils.ToDictionary(parameters), userToken);
            }
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void PostAsync(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            if (parameters is IDictionary<string, object>)
            {
                PostAsync((IDictionary<string, object>)parameters);
            }
            else if (parameters is string)
            {
                PostAsync((string)parameters, null);
            }
            else
            {
                PostAsync(FacebookUtils.ToDictionary(parameters));
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        public virtual void DeleteAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            DeleteAsync(path, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void DeleteAsync(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            DeleteAsync(path, parameters, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void DeleteAsync(string path, IDictionary<string, object> parameters, object userToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            ApiAsync(path, parameters, HttpMethod.Delete, userToken);
        }

        #endregion

        private HttpWebRequestWrapper _httpWebRequest;

        internal protected virtual void ApiAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken)
        {
            Stream input;

            bool isBatchRequest = httpMethod == HttpMethod.Post && path == null && parameters.ContainsKey("batch");
            bool isQuery = !isBatchRequest && httpMethod == HttpMethod.Get && path == "fql" && parameters != null && parameters.ContainsKey("__internal");

            if (isQuery)
            {
                parameters.Remove("__internal");
            }

            IDictionary<string, FacebookMediaObject> mediaObjects;
            var httpHelper = new HttpHelper(PrepareRequest(path, parameters, httpMethod, out input, out mediaObjects));
            _httpWebRequest = httpHelper.HttpWebRequest;

#if TPL
            if (HttpWebRequestWrapperCreated != null)
                HttpWebRequestWrapperCreated(this, new HttpWebRequestCreatedEventArgs(userToken, httpHelper.HttpWebRequest));
#endif

            bool notifyUploadProgressChanged = httpMethod == HttpMethod.Post && UploadProgressChanged != null && mediaObjects != null && mediaObjects.Count > 0;

            httpHelper.OpenReadCompleted +=
                (o, e) =>
                {
                    FacebookApiEventArgs args = null;
                    if (e.Cancelled)
                    {
                        args = new FacebookApiEventArgs(e.Error, true, userToken, null, isBatchRequest, isQuery);
                    }
                    else if (e.Error == null)
                    {
                        Exception ex;
                        string responseString;
                        bool cancelled;
                        ProcessResponse(httpHelper, e.Result, null, out responseString, out ex, out cancelled);
                        args = new FacebookApiEventArgs(ex, cancelled, userToken, responseString, isBatchRequest, isQuery);
                    }
                    else
                    {
                        if (e.Error is WebExceptionWrapper)
                        {
                            var webEx = (WebExceptionWrapper)e.Error;
                            if (webEx.GetResponse() == null)
                            {
                                args = new FacebookApiEventArgs(webEx, false, userToken, null, isBatchRequest, isQuery);
                            }
                            else
                            {
                                httpHelper.OpenReadAsync();
                                return;
                            }
                        }
                    }

                    OnCompleted(httpMethod, args);
                };

            if (input == null)
            {
                httpHelper.OpenReadAsync();
            }
            else
            {
                // we have a request body, so write it.
                httpHelper.OpenWriteCompleted +=
                    (o, e) =>
                    {
                        FacebookApiEventArgs args;
                        if (e.Cancelled)
                        {
                            // input might still be open, so dispose it.
                            DisposeInputRequestStream(input);
                            args = new FacebookApiEventArgs(e.Error, true, userToken, null, isBatchRequest, isQuery);
                        }
                        else if (e.Error == null)
                        {
                            try
                            {
                                using (var output = e.Result)
                                {
                                    byte[] buffer = new byte[1024 * 4]; // 4 kb

                                    int nread;
                                    long totalBytesToSend = input.Length;
                                    long bytesSent = 0;

                                    while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                    {
                                        output.Write(buffer, 0, nread);

                                        // notify upload progress changed if required.
                                        if (notifyUploadProgressChanged)
                                        {
                                            bytesSent += nread;
                                            OnUploadProgressChanged(new FacebookUploadProgressChangedEventArgs(0, 0, bytesSent, totalBytesToSend, ((int)(bytesSent * 100 / totalBytesToSend)), userToken));
                                        }
                                    }
                                }

                                httpHelper.OpenReadAsync();
                                return;
                            }
                            catch (WebException ex)
                            {
                                args = new FacebookApiEventArgs(new WebExceptionWrapper(ex), ex.Status == WebExceptionStatus.RequestCanceled, userToken, null, isBatchRequest, isQuery);
                            }
                            catch (WebExceptionWrapper ex)
                            {
                                args = new FacebookApiEventArgs(ex, ex.Status == WebExceptionStatus.RequestCanceled, userToken, null, isBatchRequest, isQuery);
                            }
                            catch (Exception ex)
                            {
                                args = new FacebookApiEventArgs(ex, false, userToken, null, isBatchRequest, isQuery);
                            }
                            finally
                            {
                                DisposeInputRequestStream(input);
                            }
                        }
                        else
                        {
                            DisposeInputRequestStream(input);
                            if (e.Error is WebExceptionWrapper)
                            {
                                var ex = (WebExceptionWrapper)e.Error;
                                if (ex.GetResponse() != null)
                                {
                                    httpHelper.OpenReadAsync();
                                    return;
                                }
                            }
                            args = new FacebookApiEventArgs(e.Error, false, userToken, null, isBatchRequest, isQuery);
                        }

                        OnCompleted(httpMethod, args);
                    };

                httpHelper.OpenWriteAsync();
            }
        }

#if TPL

        #region Get

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException"/>
        public virtual System.Threading.Tasks.Task<object> GetTaskAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return GetTaskAsync(path, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> GetTaskAsync(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            return GetTaskAsync(null, parameters);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> GetTaskAsync(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return GetTaskAsync(path, parameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException"/>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> GetTaskAsync(string path, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return ApiTaskAsync(path, parameters, HttpMethod.Get, null, cancellationToken);
        }

        #endregion

        #region Post

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> PostTaskAsync(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters"); ;

            return PostTaskAsync(null, parameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> PostTaskAsync(string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return PostTaskAsync(path, parameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> PostTaskAsync(string path, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return ApiTaskAsync(path, parameters, HttpMethod.Post, null, cancellationToken);
        }

#if ASYNC_AWAIT

        /// <summary>
        /// Makes an asynchronous POST request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="uploadProgress">
        /// The upload progress.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> PostTaskAsync(string path, IDictionary<string, object> parameters, object userToken, System.Threading.CancellationToken cancellationToken, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgress)
        {
            if (string.IsNullOrEmpty(path) && parameters == null)
                throw new ArgumentException("At least path or parameters must be defined.");

            return ApiTaskAsync(path, parameters, HttpMethod.Post, userToken, cancellationToken, uploadProgress);
        }

#endif

        #endregion

        #region Delete

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual System.Threading.Tasks.Task<object> DeleteTaskAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            return DeleteTaskAsync(path, null);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual System.Threading.Tasks.Task<object> DeleteTaskAsync(string path, IDictionary<string, object> parameters)
        {
            return DeleteTaskAsync(path, parameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the Facebook server.
        /// </summary>
        /// <param name="path">
        /// The resource path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        /// <exception cref="Facebook.FacebookApiException" />
        public virtual System.Threading.Tasks.Task<object> DeleteTaskAsync(string path, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken)
        {
            return ApiTaskAsync(path, parameters, HttpMethod.Delete, null, cancellationToken);
        }

        #endregion

#if ASYNC_AWAIT

        protected virtual System.Threading.Tasks.Task<object> ApiTaskAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken, System.Threading.CancellationToken cancellationToken)
        {
            return ApiTaskAsync(path, parameters, httpMethod, userToken, cancellationToken, null);
        }
#endif

        protected virtual System.Threading.Tasks.Task<object> ApiTaskAsync(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, object userToken, System.Threading.CancellationToken cancellationToken
#if ASYNC_AWAIT
, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgres
#endif
)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>(userToken);
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                HttpWebRequestWrapper httpWebRequest = null;
                EventHandler<HttpWebRequestCreatedEventArgs> httpWebRequestCreatedHandler = null;
                httpWebRequestCreatedHandler += (o, e) =>
                                                    {
                                                        if (e.UserState != tcs)
                                                            return;
                                                        httpWebRequest = e.HttpWebRequest;
                                                    };

                var ctr = cancellationToken.Register(() => { if (httpWebRequest != null) httpWebRequest.Abort(); });

#if ASYNC_AWAIT
                EventHandler<FacebookUploadProgressChangedEventArgs> uploadProgressHandler = null;
                if (uploadProgres != null)
                {

                    uploadProgressHandler = (sender, e) =>
                                            {
                                                if (e.UserState != tcs)
                                                    return;
                                                uploadProgres.Report(new FacebookUploadProgressChangedEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.BytesSent, e.TotalBytesToSend, e.ProgressPercentage, userToken));
                                            };

                    UploadProgressChanged += uploadProgressHandler;
                }
#endif

                EventHandler<FacebookApiEventArgs> handler = null;
                handler = (sender, e) =>
                        {
                            FacebookUtils.TransferCompletionToTask(tcs, e, e.GetResultData, () =>
                                                                                                {
                                                                                                    if (ctr != null) ctr.Dispose();
                                                                                                    RemoveTaskAsyncHandlers(httpMethod, handler);
                                                                                                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
#if ASYNC_AWAIT
                                                                                                    if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
#endif
                                                                                                });
                        };

                if (httpMethod == HttpMethod.Get)
                    GetCompleted += handler;
                else if (httpMethod == HttpMethod.Post)
                    PostCompleted += handler;
                else if (httpMethod == HttpMethod.Delete)
                    DeleteCompleted += handler;
                else
                    throw new ArgumentOutOfRangeException("httpMethod");

                HttpWebRequestWrapperCreated += httpWebRequestCreatedHandler;

                try
                {
                    ApiAsync(path, parameters, httpMethod, tcs);
                    if (cancellationToken.IsCancellationRequested && httpWebRequest != null) httpWebRequest.Abort();
                }
                catch
                {
                    RemoveTaskAsyncHandlers(httpMethod, handler);
                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
#if ASYNC_AWAIT
                    if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
#endif
                    throw;
                }
            }

            return tcs.Task;
        }

        private void RemoveTaskAsyncHandlers(HttpMethod httpMethod, EventHandler<FacebookApiEventArgs> handler)
        {
            if (httpMethod == HttpMethod.Get)
                GetCompleted -= handler;
            else if (httpMethod == HttpMethod.Post)
                PostCompleted -= handler;
            else if (httpMethod == HttpMethod.Delete)
                DeleteCompleted -= handler;
        }

#endif

        /// <summary>
        /// Cancels the asynchronous request.
        /// </summary>
        public void CancelAsync()
        {
            lock (this)
            {
                if (_httpWebRequest != null)
                    _httpWebRequest.Abort();
            }
        }

        #endregion

        #region Query (FQL)

#if !(SILVERLIGHT || WINRT)

        /// <summary>
        /// Executes a FQL query.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public virtual object Query(string fql)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

            var parameters = new Dictionary<string, object>();
            parameters["q"] = fql;

            var result = (IDictionary<string, object>)Get("fql", parameters);
            return result["data"];

            /*
            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";

            return Get(parameters);
            */
        }

        /// <summary>
        /// Executes a FQL multiquery.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        public virtual object Query(params string[] fql)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            var queryDict = new Dictionary<string, object>();

            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["q"] = queryDict;

            var result = (IDictionary<string, object>)Get("fql", parameters);
            return result["data"];

            /*
            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";

            return Get(parameters);
            */
        }

#endif

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        public virtual void QueryAsync(string fql)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

            QueryAsync(fql, null);
        }

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void QueryAsync(string fql, object userToken)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

            var parameters = new Dictionary<string, object>();
            parameters["q"] = fql;
            parameters["__internal"] = true;

            GetAsync("fql", parameters, userToken);

            /*            
            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";

            GetAsync(null, parameters, userToken);
            */
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        public virtual void QueryAsync(string[] fql)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            QueryAsync(fql, null);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="userToken">
        /// The user Token.
        /// </param>
        public virtual void QueryAsync(string[] fql, object userToken)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            var queryDict = new Dictionary<string, object>();

            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["q"] = queryDict;
            parameters["__internal"] = true;

            GetAsync("fql", parameters, userToken);

            /*
            var parameters = new Dictionary<string, object>();
            parameters["queries"] = queryDict;
            parameters["method"] = "fql.multiquery";

            GetAsync(null, parameters, userToken);
            */
        }


#if TPL

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> QueryTaskAsync(string fql)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

            return QueryTaskAsync(fql, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Executes a FQL query asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// The FQL query result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> QueryTaskAsync(string fql, System.Threading.CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(fql))
                throw new ArgumentNullException("fql");

            var parameters = new Dictionary<string, object>();
            parameters["q"] = fql;
            parameters["__internal"] = true;

            return GetTaskAsync("fql", parameters, cancellationToken);

            /*
            var parameters = new Dictionary<string, object>();
            parameters["query"] = fql;
            parameters["method"] = "fql.query";

            return GetTaskAsync(null, parameters, cancellationToken);
            */
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> QueryTaskAsync(params string[] fql)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            return QueryTaskAsync(fql, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Executes a FQL multiquery asynchronously.
        /// </summary>
        /// <param name="fql">
        /// The FQL queries.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="Facebook.FacebookApiException" />
        /// <returns>
        /// A collection of the FQL query results.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> QueryTaskAsync(string[] fql, System.Threading.CancellationToken cancellationToken)
        {
            if (fql == null)
                throw new ArgumentNullException("fql");
            if (fql.Length == 0)
                throw new ArgumentException("At least one fql query required.", "fql");

            var queryDict = new Dictionary<string, object>();

            for (int i = 0; i < fql.Length; i++)
            {
                queryDict.Add(string.Concat("query", i), fql[i]);
            }

            var parameters = new Dictionary<string, object>();
            parameters["q"] = queryDict;
            parameters["__internal"] = true;

            return GetTaskAsync("fql", parameters, cancellationToken);
        }

#endif

        #endregion

        #region Batch Requests

#if !(SILVERLIGHT || WINRT)

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual object Batch(params FacebookBatchParameter[] batchParameters)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            return ProcessBatchResult(Post(PrepareBatchParameter(batchParameters)));
        }

#endif

        /// <summary>
        /// Executes a batch request asynchronously.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <param name="userToken">
        /// The user token.
        /// </param>
        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters, object userToken)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            PostAsync(null, PrepareBatchParameter(batchParameters), userToken);
        }

        /// <summary>
        /// Executes a batch request asynchronously.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        public virtual void BatchAsync(FacebookBatchParameter[] batchParameters)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            BatchAsync(batchParameters, null);
        }


#if TPL

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> BatchTaskAsync(params FacebookBatchParameter[] batchParameters)
        {
            return BatchTaskAsync(batchParameters, System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, System.Threading.CancellationToken cancellationToken)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            return BatchTaskAsync(batchParameters, null, System.Threading.CancellationToken.None);
        }

#if ASYNC_AWAIT

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, System.Threading.CancellationToken cancellationToken)
        {
            return BatchTaskAsync(batchParameters, userToken, cancellationToken, null);
        }
#endif

        /// <summary>
        /// Executes a batch request.
        /// </summary>
        /// <param name="batchParameters">
        /// The batch parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="uploadProgres">
        /// The upload progress.
        /// </param>
        /// <returns>
        /// The json result.
        /// </returns>
        public virtual System.Threading.Tasks.Task<object> BatchTaskAsync(FacebookBatchParameter[] batchParameters, object userToken, System.Threading.CancellationToken cancellationToken
#if ASYNC_AWAIT
, IProgress<FacebookUploadProgressChangedEventArgs> uploadProgres
#endif
)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                HttpWebRequestWrapper httpWebRequest = null;
                EventHandler<HttpWebRequestCreatedEventArgs> httpWebRequestCreatedHandler = null;
                httpWebRequestCreatedHandler += (o, e) =>
                                                        {
                                                            if (e.UserState != tcs)
                                                                return;
                                                            httpWebRequest = e.HttpWebRequest;
                                                        };

                var ctr = cancellationToken.Register(() => { if (httpWebRequest != null) httpWebRequest.Abort(); });

#if ASYNC_AWAIT
                EventHandler<FacebookUploadProgressChangedEventArgs> uploadProgressHandler = null;
                if (uploadProgres != null)
                {

                    uploadProgressHandler = (sender, e) =>
                    {
                        if (e.UserState != tcs)
                            return;
                        uploadProgres.Report(new FacebookUploadProgressChangedEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.BytesSent, e.TotalBytesToSend, e.ProgressPercentage, userToken));
                    };

                    UploadProgressChanged += uploadProgressHandler;
                }
#endif

                EventHandler<FacebookApiEventArgs> handler = null;
                handler = (sender, e) => FacebookUtils.TransferCompletionToTask(tcs, e, e.GetResultData, () =>
                                                                                        {
                                                                                            if (ctr != null) ctr.Dispose();
                                                                                            PostCompleted -= handler;
                                                                                            HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
#if ASYNC_AWAIT
                                                                                            if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
#endif
                                                                                        });
                PostCompleted += handler;
                HttpWebRequestWrapperCreated += httpWebRequestCreatedHandler;

                try
                {
                    BatchAsync(batchParameters, tcs);
                    if (cancellationToken.IsCancellationRequested && httpWebRequest != null) httpWebRequest.Abort();
                }
                catch
                {
                    PostCompleted -= handler;
                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
#if ASYNC_AWAIT
                    if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
#endif
                    throw;
                }
            }

            return tcs.Task;
        }

#endif

        internal IDictionary<string, object> PrepareBatchParameter(FacebookBatchParameter[] batchParameters)
        {
            if (batchParameters == null)
                throw new ArgumentNullException("batchParameters");
            if (batchParameters.Length == 0)
                throw new ArgumentException("At least one batchParameter required.", "batchParameters");

            var actualParameters = new Dictionary<string, object>();
            var parameters = new List<object>();

            foreach (var parameter in batchParameters)
            {
                IDictionary<string, FacebookMediaObject> mediaObjects;
                parameters.Add(ToParameters(parameter, out mediaObjects));

                if (mediaObjects != null)
                {
                    foreach (var facebookMediaObject in mediaObjects)
                    {
                        actualParameters.Add(facebookMediaObject.Key, facebookMediaObject.Value);
                    }
                }
            }

            actualParameters["batch"] = JsonSerializer.Current.SerializeObject(parameters);

            return actualParameters;
        }

        /// <summary>
        /// Converts the facebook batch to POST parameters.
        /// </summary>
        /// <param name="batchParameter">
        /// The batch parameter.
        /// </param>
        /// <returns>
        /// The post parameters.
        /// </returns>
        protected IDictionary<string, object> ToParameters(FacebookBatchParameter batchParameter, out IDictionary<string, FacebookMediaObject> mediaObjects)
        {
            if (batchParameter == null)
                throw new ArgumentNullException("batchParameter");

            mediaObjects = null;
            IDictionary<string, object> returnResult;

            var defaultParameters = new Dictionary<string, object>();

            defaultParameters["method"] = FacebookUtils.ConvertToStringForce(batchParameter.HttpMethod);

            IDictionary<string, object> parameters;
            if (batchParameter.Parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            else
            {
                if (batchParameter.Parameters is IDictionary<string, object>)
                {
                    parameters = (IDictionary<string, object>)batchParameter.Parameters;
                    mediaObjects = ExtractMediaObjects(parameters);
                    if (mediaObjects.Count == 1)
                    {
                        defaultParameters["attached_files"] = mediaObjects.ElementAt(0).Key;
                    }
                }
                else
                {
                    parameters = FacebookUtils.ToDictionary(batchParameter.Parameters);
                }
            }

            var path = FacebookUtils.ParseQueryParametersToDictionary(batchParameter.Path, parameters);
            string queryString = string.Empty;
            if (batchParameter.HttpMethod == HttpMethod.Get)
            {
                queryString = FacebookUtils.ToJsonQueryString(parameters);
            }
            else
            {
                defaultParameters["body"] = FacebookUtils.ToJsonQueryString(parameters);
            }

            var relativeUrl = new StringBuilder(path);
            if (!string.IsNullOrEmpty(queryString))
            {
                relativeUrl.AppendFormat("?{0}", queryString);
            }

            defaultParameters["relative_url"] = relativeUrl.ToString();

            var data = batchParameter.Data;
            if (data == null)
            {
                returnResult = defaultParameters;
            }
            else
            {
                if (!(data is IDictionary<string, object>))
                {
                    data = FacebookUtils.ToDictionary(batchParameter.Data);
                }

                returnResult = FacebookUtils.Merge(defaultParameters, (IDictionary<string, object>)data);
            }

            return returnResult;
        }

        /// <summary>
        /// Processes the batch result.
        /// </summary>
        /// <param name="result">
        /// The json result.
        /// </param>
        /// <returns>
        /// Batch result.
        /// </returns>
        internal static object ProcessBatchResult(object result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            IList<object> list = new JsonArray();

            var resultList = (IList<object>)result;

            foreach (var row in resultList)
            {
                if (row == null)
                {
                    // row is null when omit_response_on_success = true
                    list.Add(null);
                }
                else
                {
                    var body = (string)((IDictionary<string, object>)row)["body"];
                    object jsonObject;

                    var exception = ExceptionFactory.GetGraphException(body, out jsonObject) ??
                                    ExceptionFactory.GetRestException(jsonObject);

                    list.Add(exception ?? jsonObject);
                }
            }

            return list;
        }

        #endregion

        private HttpWebRequestWrapper PrepareRequest(string path, IDictionary<string, object> parameters, HttpMethod httpMethod, out Stream input, out IDictionary<string, FacebookMediaObject> mediaObjects)
        {
            parameters = AddReturnSslResourceIfRequired(parameters, IsSecureConnection);
            mediaObjects = null;

            // Authenticate request
            if (!parameters.ContainsKey("access_token") && !string.IsNullOrEmpty(AccessToken))
                parameters["access_token"] = AccessToken;

            var urlBuilder = new UriBuilder(BuildRootUrl(httpMethod, path, parameters));

            string contentType = null;
            var queryString = new StringBuilder();

            if (httpMethod == HttpMethod.Get)
            {
                // for GET, all parameters goes as querystrings
                input = null;
                queryString.Append(FacebookUtils.ToJsonQueryString(parameters));
            }
            else
            {
                if (parameters.ContainsKey("access_token"))
                {
                    queryString.AppendFormat("access_token={0}", parameters["access_token"]);
                    parameters.Remove("access_token");
                }

#if SILVERLIGHT
                if (httpMethod == HttpMethod.Delete)
                {
                    if (queryString.Length > 0)
                        queryString.Append("&");
                    queryString.Append("method=delete");
                }
#endif

                mediaObjects = ExtractMediaObjects(parameters);
                if (mediaObjects.Count == 0)
                {
                    contentType = "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(FacebookUtils.ToJsonQueryString(parameters));
                    input = data.Length == 0 ? null : new MemoryStream(data);
                }
                else
                {
                    string boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);
                    contentType = string.Concat("multipart/form-data; boundary=", boundary);
                    input = BuildMediaObjectRequestBody(parameters, mediaObjects, boundary);
                }
            }

            urlBuilder.Query = queryString.ToString();

            var httpWebRequest = CreateHttpWebRequest(urlBuilder.Uri);
            httpWebRequest.Method = FacebookUtils.ConvertToString(httpMethod);
            httpWebRequest.ContentType = contentType;

            if (input != null)
                httpWebRequest.TrySetContentLength(input.Length);

            return httpWebRequest;
        }

        private object ProcessResponse(HttpHelper httpHelper, Stream responseStream, Type resultType, out string responseStr, out Exception exception, out bool cancelled)
        {
            responseStr = null;
            cancelled = true;

            try
            {
                using (var stream = responseStream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseStr = reader.ReadToEnd();
                    }
                }

                cancelled = false;

                object json;
                exception = ExceptionFactory.GetException(DomainMaps, httpHelper.HttpWebRequest.RequestUri, responseStr, httpHelper.InnerException, out json);

                if (exception != null)
                    return null;

                // ExceptionFactory already deserialized json, so use that instead incase resultType is null
                if (resultType != null)
                    json = JsonSerializer.Current.DeserializeObject(responseStr, resultType);

                return json;
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        private void DisposeInputRequestStream(Stream input)
        {
            if (input == null)
            {
                return;
            }

            if (input is CombinationStream.CombinationStream)
            {
                var cs = (CombinationStream.CombinationStream)input;
                foreach (var stream in cs.InternalStreams)
                    stream.Dispose();
                cs.Dispose();
            }
            else
            {
                input.Dispose();
            }
        }

        /// <summary>
        /// Creates the http web request.
        /// </summary>
        /// <param name="url">The url of the http web request.</param>
        /// <returns>The http helper.</returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual HttpWebRequestWrapper CreateHttpWebRequest(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            return new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(url));
        }

        internal static IDictionary<string, object> AddReturnSslResourceIfRequired(IDictionary<string, object> parameters, bool isSecuredConnection)
        {
            var mergedParameters = FacebookUtils.Merge(null, parameters);

            if (isSecuredConnection && !mergedParameters.ContainsKey(FacebookUtils.Resources.return_ssl_resources))
            {
                mergedParameters[FacebookUtils.Resources.return_ssl_resources] = true;
            }

            return mergedParameters;
        }

        #endregion

        private Uri BuildRootUrl(HttpMethod httpMethod, string path, IDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            path = FacebookUtils.ParseQueryParametersToDictionary(path, parameters);

            if (parameters.ContainsKey("method"))
            {
                // legacy rest api
                if (string.IsNullOrEmpty((string)parameters["method"]))
                    throw new ArgumentException(FacebookUtils.Resources.ParameterMethodValueRequired);

                // Set the format to json
                parameters["format"] = "json-strings";
                return GetApiUrl(parameters["method"].ToString());
            }

            // graph api
            return !string.IsNullOrEmpty(path) && httpMethod == HttpMethod.Post && path.EndsWith("/videos")
                       ? GetUrl(FacebookUtils.DOMAIN_MAP_GRAPH_VIDEO, path, null)
                       : GetUrl(FacebookUtils.DOMAIN_MAP_GRAPH, path, null);
        }

        internal static IDictionary<string, FacebookMediaObject> ExtractMediaObjects(IDictionary<string, object> parameters)
        {
            var mediaObjects = new Dictionary<string, FacebookMediaObject>();

            if (parameters == null)
                return mediaObjects;

            foreach (var parameter in parameters)
            {
                if (parameter.Value is FacebookMediaObject)
                    mediaObjects.Add(parameter.Key, (FacebookMediaObject)parameter.Value);
            }

            foreach (var mediaObject in mediaObjects)
                parameters.Remove(mediaObject.Key);

            return mediaObjects;
        }

        internal Stream BuildMediaObjectRequestBody(IDictionary<string, object> parameters, IDictionary<string, FacebookMediaObject> mediaObjects, string boundary)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (mediaObjects == null)
                throw new ArgumentNullException("mediaObjects");
            if (mediaObjects.Count == 0)
                throw new ArgumentException("MediaObjects empty.", "mediaObjects");
            if (string.IsNullOrEmpty(boundary))
                throw new ArgumentNullException("boundary");

            var streams = new List<Stream>();

            // Build up the post message header
            var sb = new StringBuilder();

            foreach (var kvp in parameters)
            {
                sb.Append(FacebookUtils.MultiPartFormPrefix).Append(boundary).Append(FacebookUtils.MultiPartNewLine);
                sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
                sb.Append(FacebookUtils.MultiPartNewLine);
                sb.Append(FacebookUtils.MultiPartNewLine);

                // format object As json And Remove leading and trailing parenthesis
                string jsonValue = FacebookUtils.ToJsonString(kvp.Value);

                sb.Append(jsonValue);
                sb.Append(FacebookUtils.MultiPartNewLine);
            }

            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())));

            foreach (var facebookMediaObject in mediaObjects)
            {
                var sbMediaObject = new StringBuilder();
                var mediaObject = facebookMediaObject.Value;

                if (mediaObject.ContentType == null || mediaObject.GetValue() == null || string.IsNullOrEmpty(mediaObject.FileName))
                {
                    throw new InvalidOperationException(FacebookUtils.Resources.MediaObjectMustHavePropertiesSetError);
                }

                sbMediaObject.Append(FacebookUtils.MultiPartFormPrefix).Append(boundary).Append(FacebookUtils.MultiPartNewLine);
                sbMediaObject.Append("Content-Disposition: form-data; name=\"").Append(facebookMediaObject.Key).Append("\"; filename=\"").Append(mediaObject.FileName).Append("\"").Append(FacebookUtils.MultiPartNewLine);
                sbMediaObject.Append("Content-Type: ").Append(mediaObject.ContentType).Append(FacebookUtils.MultiPartNewLine).Append(FacebookUtils.MultiPartNewLine);

                streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(sbMediaObject.ToString())));

                byte[] fileData = mediaObject.GetValue();

                Debug.Assert(fileData != null, "The value of FacebookMediaObject is null.");

                streams.Add(new MemoryStream(fileData));
                streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(FacebookUtils.MultiPartNewLine)));
            }

            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(String.Concat(FacebookUtils.MultiPartNewLine, FacebookUtils.MultiPartFormPrefix, boundary, FacebookUtils.MultiPartFormPrefix, FacebookUtils.MultiPartNewLine))));

            return new CombinationStream.CombinationStream(streams);
        }

        protected virtual Uri GetUrl(string name, string path, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return FacebookUtils.GetUrl(DomainMaps, name, path, parameters);
        }

        protected virtual Uri GetApiUrl(string method)
        {
            if (string.IsNullOrEmpty(method))
                throw new ArgumentNullException("method");

            method = method.ToLowerInvariant();

            string name;

            if (method.Equals("video.upload"))
                name = "api_video";
            else if (FacebookUtils.ReadOnlyCalls.Contains(method))
                name = "api_read";
            else
                name = "api";

            return GetUrl(name, "restserver.php", null);
        }

        protected virtual void OnGetCompleted(FacebookApiEventArgs args)
        {
            if (GetCompleted != null)
                GetCompleted(this, args);
        }

        protected virtual void OnPostCompleted(FacebookApiEventArgs args)
        {
            if (PostCompleted != null)
                PostCompleted(this, args);
        }

        protected virtual void OnDeleteCompleted(FacebookApiEventArgs args)
        {
            if (DeleteCompleted != null)
                DeleteCompleted(this, args);
        }

        protected void OnUploadProgressChanged(FacebookUploadProgressChangedEventArgs args)
        {
            if (UploadProgressChanged != null)
                UploadProgressChanged(this, args);
        }

        private void OnCompleted(HttpMethod httpMethod, FacebookApiEventArgs args)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    OnGetCompleted(args);
                    break;
                case HttpMethod.Post:
                    OnPostCompleted(args);
                    break;
                case HttpMethod.Delete:
                    OnDeleteCompleted(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }
        }
    }
}