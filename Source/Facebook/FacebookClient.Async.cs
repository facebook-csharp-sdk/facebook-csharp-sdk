// --------------------------------
// <copyright file="FacebookClient.Async.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.IO;

    public partial class FacebookClient
    {
        private HttpWebRequestWrapper _httpWebRequest;
        private object _httpWebRequestLocker = new object();

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

        /// <summary>
        /// Cancels asynchronous requests.
        /// </summary>
        /// <remarks>
        /// Does not cancel requests created using XTaskAsync methods.
        /// </remarks>
        public virtual void CancelAsync()
        {
            lock (_httpWebRequestLocker)
            {
                if (_httpWebRequest != null)
                    _httpWebRequest.Abort();
            }
        }

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void ApiAsync(string httpMethod, string path, object parameters, Type resultType, object userState)
        {
            Stream input;
            bool isLegacyRestApi;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input, out isLegacyRestApi);

            var uploadProgressChanged = UploadProgressChanged;
            bool notifyUploadProgressChanged = uploadProgressChanged != null && httpHelper.HttpWebRequest.Method == "POST";

            httpHelper.OpenReadCompleted +=
                (o, e) =>
                {
                    throw new NotImplementedException();
                };

            if (input == null)
            {
                httpHelper.OpenReadAsync();
            }
            else
            {
                // we have a request body so write
                httpHelper.OpenWriteCompleted +=
                    (o, e) =>
                    {
                        FacebookApiEventArgs args;
                        if (e.Cancelled)
                        {
                            input.Dispose();
                            args = new FacebookApiEventArgs(e.Error, true, userState, null);
                        }
                        else if (e.Error == null)
                        {
                            try
                            {
                                using (var stream = e.Result)
                                {
                                    // write input to requestStream
                                    var buffer = new byte[BufferSize];
                                    int nread;

                                    if (notifyUploadProgressChanged)
                                    {
                                        long totalBytesToSend = input.Length;
                                        long bytesSent = 0;

                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();

                                            // notify upload progress changed
                                            bytesSent += nread;
                                            OnUploadProgressChanged(new FacebookUploadProgressChangedEventArgs(0, 0, bytesSent, totalBytesToSend, ((int)(bytesSent * 100 / totalBytesToSend)), userState));
                                        }
                                    }
                                    else
                                    {
                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();
                                        }
                                    }
                                }
                                httpHelper.OpenReadAsync();
                                return;
                            }
                            catch (Exception ex)
                            {
                                // if canncelled
                                throw new NotImplementedException();
                            }
                            finally
                            {
                                input.Dispose();
                            }
                        }
                        else
                        {
                            input.Dispose();
                            if (e.Error is WebExceptionWrapper)
                            {
                                var ex = (WebExceptionWrapper)e.Error;
                                if (ex.GetResponse() != null)
                                {
                                    httpHelper.OpenReadAsync(); // todo: can it be cancelled here?
                                    return;
                                }
                            }

                            args = new FacebookApiEventArgs(e.Error, false, userState, null);  // todo: could had been cancelled
                        }

                        OnCompleted(httpMethod, args);
                    };

                httpHelper.OpenWriteAsync();
            }
        }

        #region Events

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

        private void OnCompleted(string httpMethod, FacebookApiEventArgs args)
        {
            switch (httpMethod)
            {
                case "GET":
                    OnGetCompleted(args);
                    break;
                case "POST":
                    OnPostCompleted(args);
                    break;
                case "DELETE":
                    OnDeleteCompleted(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }
        }
        #endregion

        /// <summary>
        /// Makes an asynchronous request to the Facebook server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <typeparam name="T">The type of deserialize object into.</typeparam>
#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void ApiAsync<T>(string httpMethod, string path, object parameters, object userState)
        {
            ApiAsync(httpMethod, path, parameters, typeof(T), userState);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]        
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path)
        {
            GetAsync(path, null, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(object parameters)
        {
            GetAsync(null, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters)
        {
            GetAsync(path, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void GetAsync(string path, object parameters, object userState)
        {
            ApiAsync("GET", path, parameters, null, userState);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(object parameters)
        {
            PostAsync(null, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters)
        {
            PostAsync(path, parameters, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void PostAsync(string path, object parameters, object userState)
        {
            ApiAsync("POST", path, parameters, null, userState);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void DeleteAsync(string path)
        {
            DeleteAsync(path, null, null);
        }

#if FLUENTHTTP_CORE_TPL
        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public virtual void DeleteAsync(string path, object parameters, object userState)
        {
            ApiAsync("DELETE", path, parameters, null, userState);
        }
    }
}