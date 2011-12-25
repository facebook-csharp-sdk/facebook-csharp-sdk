// Fluent HTTP CORE
// Copyright 2011. Prabir Shrestha (www.prabir.me)
// Apache License, Version 2.0

//#define FLUENTHTTP_CORE_UTILS
//#define FLUENTHTTP_CORE_TPL
//#define FLUENTHTTP_CORE_WINRT
//#define FLUENTHTTP_CORE_STREAM
//#define FLUENTHTTP_URLENCODING
//#define FLUENTHTTP_HTMLENCODING
//#define FLUENTHTTP_HTTPBASIC_AUTHENTICATION

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
#if FLUENTHTTP_CORE_TPL
using System.Threading.Tasks;
#endif


namespace FluentHttp
{
    /// <summary>
    /// Represents the http web request wrapper.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HttpWebRequestWrapper
    {
        /// <summary>
        /// The http web request.
        /// </summary>
        private readonly HttpWebRequest _httpWebRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebRequestWrapper"/> class.
        /// </summary>
        protected HttpWebRequestWrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebRequestWrapper"/> class.
        /// </summary>
        /// <param name="httpWebRequest">
        /// The http web request.
        /// </param>
        public HttpWebRequestWrapper(HttpWebRequest httpWebRequest)
        {
            if (httpWebRequest == null)
                throw new ArgumentNullException("httpWebRequest");

            _httpWebRequest = httpWebRequest;
        }

        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public virtual string Method
        {
            get { return _httpWebRequest.Method; }
            set { _httpWebRequest.Method = value; }
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public virtual string ContentType
        {
            get { return _httpWebRequest.ContentType; }
            set { _httpWebRequest.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets the http headers.
        /// </summary>
        public virtual WebHeaderCollection Headers
        {
            get { return _httpWebRequest.Headers; }
            set { _httpWebRequest.Headers = value; }
        }

#if !(WINDOWS_PHONE || FLUENTHTTP_CORE_WINRT)

        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public virtual long ContentLength
        {
            get { return _httpWebRequest.ContentLength; }
            set { _httpWebRequest.ContentLength = value; }
        }
#endif

#if !FLUENTHTTP_CORE_WINRT
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public virtual string UserAgent
        {
            get { return _httpWebRequest.UserAgent; }
            set { _httpWebRequest.UserAgent = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the cookie container.
        /// </summary>
        public virtual CookieContainer CookieContainer
        {
            get { return _httpWebRequest.CookieContainer; }
            set { _httpWebRequest.CookieContainer = value; }
        }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public virtual ICredentials Credentials
        {
            get { return _httpWebRequest.Credentials; }
            set { _httpWebRequest.Credentials = value; }
        }

        /// <summary>
        /// Gets the request uri.
        /// </summary>
        public virtual Uri RequestUri
        {
            get { return _httpWebRequest.RequestUri; }
        }

        /// <summary>
        /// Gets or sets the accept.
        /// </summary>
        public virtual string Accept
        {
            get { return _httpWebRequest.Accept; }
            set { _httpWebRequest.Accept = value; }
        }

#if !SILVERLIGHT

#if !FLUENTHTTP_CORE_WINRT

        public virtual ServicePoint ServicePoint
        {
            get { return _httpWebRequest.ServicePoint; }
        }

        /// <summary>
        /// Gets or sets the decompression method.
        /// </summary>
        public virtual DecompressionMethods AutomaticDecompression
        {
            get { return _httpWebRequest.AutomaticDecompression; }
            set { _httpWebRequest.AutomaticDecompression = value; }
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public virtual string Connection
        {
            get { return _httpWebRequest.Connection; }
            set { _httpWebRequest.Connection = value; }
        }

        /// <summary>
        /// Gets or sets the expect.
        /// </summary>
        public virtual string Expect
        {
            get { return _httpWebRequest.Expect; }
            set { _httpWebRequest.Expect = value; }
        }

        /// <summary>
        /// Gets or sets the if modified since.
        /// </summary>
        public virtual DateTime IfModifiedSince
        {
            get { return _httpWebRequest.IfModifiedSince; }
            set { _httpWebRequest.IfModifiedSince = value; }
        }

        /// <summary>
        /// Gets or sets the read write timeout.
        /// </summary>
        public virtual int ReadWriteTimeout
        {
            get { return _httpWebRequest.ReadWriteTimeout; }
            set { _httpWebRequest.ReadWriteTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the referrer.
        /// </summary>
        public virtual string Referer
        {
            get { return _httpWebRequest.Referer; }
            set { _httpWebRequest.Referer = value; }
        }

        /// <summary>
        /// Gets or set the time in milliseconds, before the request times out.
        /// </summary>
        public virtual int Timeout
        {
            get { return _httpWebRequest.Timeout; }
            set { _httpWebRequest.Timeout = value; }
        }

        /// <summary>
        /// Gets or sets the transfer encoding.
        /// </summary>
        public virtual string TransferEncoding
        {
            get { return _httpWebRequest.TransferEncoding; }
            set { _httpWebRequest.TransferEncoding = value; }
        }

#endif

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        public virtual IWebProxy Proxy
        {
            get { return _httpWebRequest.Proxy; }
            set { _httpWebRequest.Proxy = value; }
        }

#endif
        /// <summary>
        /// Try to set the value of the content length header.
        /// </summary>
        /// <param name="contentLength">
        /// The Content-Length value.
        /// </param>
        /// <returns>
        /// Returns true if ContentLength set, otherwise false.
        /// </returns>
        public virtual bool TrySetContentLength(long contentLength)
        {
#if !(WINDOWS_PHONE || FLUENTHTTP_CORE_WINRT)
            ContentLength = contentLength;
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Begins the get response.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The async result.
        /// </returns>
        public virtual IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return _httpWebRequest.BeginGetResponse(callback, state);
        }

        /// <summary>
        /// Begins getting the request stream.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The async result.
        /// </returns>
        public virtual IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return _httpWebRequest.BeginGetRequestStream(callback, state);
        }

        /// <summary>
        /// Ends the http web get response.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The http web response.
        /// </returns>
        public virtual HttpWebResponseWrapper EndGetResponse(IAsyncResult asyncResult)
        {
            var httpWebResponse = (HttpWebResponse)_httpWebRequest.EndGetResponse(asyncResult);

            return new HttpWebResponseWrapper(httpWebResponse);
        }

        /// <summary>
        /// Ends the get request stream.
        /// </summary>
        /// <param name="asyncResult">
        /// The async result.
        /// </param>
        /// <returns>
        /// The request stream.
        /// </returns>
        public virtual Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return _httpWebRequest.EndGetRequestStream(asyncResult);
        }

#if FLUENTHTTP_CORE_TPL

        public virtual Task<HttpWebResponseWrapper> GetResponseAsync()
        {
            return Task<HttpWebResponseWrapper>.Factory.FromAsync(BeginGetResponse, EndGetResponse, null);
        }

        public virtual Task<Stream> GetRequestStreamAsync()
        {
            return Task<Stream>.Factory.FromAsync(BeginGetRequestStream, EndGetRequestStream, null);
        }

#endif

#if !(SILVERLIGHT || FLUENTHTTP_CORE_WINRT)

        public virtual HttpWebResponseWrapper GetResponse()
        {
            return new HttpWebResponseWrapper((HttpWebResponse)_httpWebRequest.GetResponse());
        }

        public virtual Stream GetRequestStream()
        {
            return _httpWebRequest.GetRequestStream();
        }

#endif
        public virtual void Abort()
        {
            _httpWebRequest.Abort();
        }

    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class HttpWebResponseWrapper
    {
        /// <summary>
        /// The http web response.
        /// </summary>
        private readonly HttpWebResponse _httpWebResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebResponseWrapper"/> class.
        /// </summary>
        protected HttpWebResponseWrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebResponseWrapper"/> class.
        /// </summary>
        /// <param name="httpWebResponse">
        /// The http web response.
        /// </param>
        public HttpWebResponseWrapper(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse == null)
                throw new ArgumentNullException("httpWebResponse");

            _httpWebResponse = httpWebResponse;
        }

        /// <summary>
        /// Gets the http method.
        /// </summary>
        public virtual string Method
        {
            get { return _httpWebResponse.Method; }
        }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        public virtual long ContentLength
        {
            get { return _httpWebResponse.ContentLength; }
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public virtual string ContentType
        {
            get { return _httpWebResponse.ContentType; }
        }

        /// <summary>
        /// Gets the response uri.
        /// </summary>
        public virtual Uri ResponseUri
        {
            get { return _httpWebResponse.ResponseUri; }
        }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        public virtual HttpStatusCode StatusCode
        {
            get { return _httpWebResponse.StatusCode; }
        }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        public virtual string StatusDescription
        {
            get { return _httpWebResponse.StatusDescription; }
        }

#if !SILVERLIGHT

        /// <summary>
        /// Gets the http cookies.
        /// </summary>
        public virtual CookieCollection Cookies
        {
            get { return _httpWebResponse.Cookies; }
        }

        /// <summary>
        /// Gets the http headers.
        /// </summary>
        public virtual WebHeaderCollection Headers
        {
            get { return _httpWebResponse.Headers; }
        }

#if !FLUENTHTTP_CORE_WINRT

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        public virtual string ContentEncoding
        {
            get { return _httpWebResponse.ContentEncoding; }
        }

        /// <summary>
        /// Gets the character set.
        /// </summary>
        public virtual string CharacterSet
        {
            get { return _httpWebResponse.CharacterSet; }
        }

        /// <summary>
        /// Gets a value indicating whether response is mutually authenticated.
        /// </summary>
        public virtual bool IsMutuallyAuthenticated
        {
            get { return _httpWebResponse.IsMutuallyAuthenticated; }
        }

        /// <summary>
        /// Gets the last modified date and time.
        /// </summary>
        public virtual DateTime LastModified
        {
            get { return _httpWebResponse.LastModified; }
        }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        public virtual Version ProtocolVersion
        {
            get { return _httpWebResponse.ProtocolVersion; }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public virtual string Server
        {
            get { return _httpWebResponse.Server; }
        }

#endif

#endif
        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <returns>
        /// The response stream.
        /// </returns>
        public virtual Stream GetResponseStream()
        {
            return _httpWebResponse.GetResponseStream();
        }
    }

#if !(FLUENTHTTP_CORE_WINRT || SILVERLIGHT)
    [Serializable]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class WebExceptionWrapper : Exception
    {
        private readonly WebException _webException;
        private readonly WebExceptionStatus _status = WebExceptionStatus.UnknownError;

        protected WebExceptionWrapper() { }

        public WebExceptionWrapper(WebException webException)
            : base(webException == null ? null : webException.Message, webException == null ? null : webException.InnerException)
        {
            _webException = webException;
            _status = webException == null ? WebExceptionStatus.UnknownError : webException.Status;
        }

#if !(FLUENTHTTP_CORE_WINRT || SILVERLIGHT)
        /// <summary>
        /// Initializes a new instance of the <see cref="WebExceptionWrapper"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected WebExceptionWrapper(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public virtual HttpWebResponseWrapper GetResponse()
        {
            return _webException.Response == null
                       ? null
                       : new HttpWebResponseWrapper((HttpWebResponse)_webException.Response);
        }

        public virtual WebException ActualWebException
        {
            get { return _webException; }
        }

        public virtual WebExceptionStatus Status
        {
            get { return _status; }
        }
    }

}

namespace FluentHttp
{
    internal delegate void OpenReadCompletedEventHandler(object sender, OpenReadCompletedEventArgs e);

    internal delegate void OpenWriteCompletedEventHandler(object sender, OpenWriteCompletedEventArgs e);

    internal class OpenReadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly Stream _result;

        public OpenReadCompletedEventArgs(Stream result, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        public Stream Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _result;
            }
        }
    }

    internal class OpenWriteCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly Stream _result;

        public OpenWriteCompletedEventArgs(Stream result, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        public Stream Result
        {
            get
            {
                RaiseExceptionIfNecessary();
                return _result;
            }
        }
    }

    internal class HttpHelper
    {
        /// <summary>
        /// Gets the inner exception.
        /// </summary>
        public Exception InnerException
        {
            get { return _innerException; }
        }

        public int Timeout { get; set; }

        public event OpenReadCompletedEventHandler OpenReadCompleted;
        public event OpenWriteCompletedEventHandler OpenWriteCompleted;

        private readonly HttpWebRequestWrapper _httpWebRequest;
        private HttpWebResponseWrapper _httpWebResponse;
        private Exception _innerException;

        public HttpHelper(Uri url)
            : this(new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(url)))
        {
        }

        public HttpHelper(string url)
            : this(new Uri(url))
        {
        }

        public HttpHelper(HttpWebRequestWrapper httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        public HttpWebRequestWrapper HttpWebRequest
        {
            get { return _httpWebRequest; }
        }

        public HttpWebResponseWrapper HttpWebResponse
        {
            get { return _httpWebResponse; }
        }

#if !(SILVERLIGHT || FLUENTHTTP_CORE_WINRT)

        public virtual Stream OpenWrite()
        {
            try
            {
                return _httpWebRequest.GetRequestStream();
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                    _httpWebResponse = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
                _innerException = webException;
                throw new WebExceptionWrapper(webException);
            }
            catch (WebExceptionWrapper webException)
            {
                _httpWebResponse = webException.GetResponse();
                _innerException = webException;
                throw;
            }
            catch (Exception ex)
            {
                _innerException = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                throw _innerException;
            }
        }

        public virtual Stream OpenRead()
        {
            try
            {
                if (_httpWebResponse == null)
                    _httpWebResponse = _httpWebRequest.GetResponse();
                return _httpWebResponse.GetResponseStream();
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                    _httpWebResponse = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
                _innerException = new WebExceptionWrapper(webException);
                throw _innerException;
            }
            catch (WebExceptionWrapper webException)
            {
                _httpWebResponse = webException.GetResponse();
                _innerException = webException;
                throw _innerException;
            }
            catch (Exception ex)
            {
                _innerException = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                throw _innerException;
            }
        }

#endif

        public virtual void OpenReadAsync(object userToken)
        {
            if (_httpWebResponse == null)
            {
                WebExceptionWrapper webExceptionWrapper = null;
                try
                {
                    IAsyncResult asyncResult = _httpWebRequest.BeginGetResponse(ar => ResponseCallback(ar, userToken), null);

                    int timeout = 0;

#if !(SILVERLIGHT || FLUENTHTTP_CORE_WINRT)
                    if (HttpWebRequest.Timeout > 0)
                        timeout = HttpWebRequest.Timeout;
#endif

                    if (Timeout > 0)
                        timeout = Timeout;

                    if (timeout > 0)
                        ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, ScanTimoutCallback, userToken, timeout, true);
                }
                catch (WebException webException)
                {
                    if (webException.Response != null)
                        _httpWebResponse = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
                    webExceptionWrapper = new WebExceptionWrapper(webException);
                    _innerException = webExceptionWrapper;
                }
                catch (WebExceptionWrapper webException)
                {
                    _httpWebResponse = webException.GetResponse();
                    webExceptionWrapper = webException;
                    _innerException = webExceptionWrapper;
                }
                catch (Exception ex)
                {
                    webExceptionWrapper = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                    _innerException = webExceptionWrapper;
                }
                finally
                {
                    if (webExceptionWrapper != null)
                        OnOpenReadCompleted(new OpenReadCompletedEventArgs(null, webExceptionWrapper, webExceptionWrapper.Status == WebExceptionStatus.RequestCanceled, userToken));
                }
            }
            else
                ResponseCallback(null, userToken);
        }

        public virtual void OpenReadAsync()
        {
            OpenReadAsync(null);
        }

        public virtual void OpenWriteAsync(object userToken)
        {
            WebExceptionWrapper webExceptionWrapper = null;

            try
            {
                _httpWebRequest.BeginGetRequestStream(
                    ar =>
                    {
                        Stream stream = null;
                        WebExceptionWrapper exception = null;

                        try
                        {
                            stream = _httpWebRequest.EndGetRequestStream(ar);
                        }
                        catch (WebException webException)
                        {
                            if (webException.Response != null)
                                _httpWebResponse = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
                            exception = new WebExceptionWrapper(webException);
                            _innerException = webException;
                        }
                        catch (WebExceptionWrapper webException)
                        {
                            _httpWebResponse = webException.GetResponse();
                            exception = webException;
                            _innerException = webException;
                        }
                        catch (Exception ex)
                        {
                            exception = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                            _innerException = exception;
                        }

                        OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(stream, exception, exception != null && exception.Status == WebExceptionStatus.RequestCanceled, userToken));
                    }, userToken);
            }
            catch (WebException webException)
            {
                webExceptionWrapper = new WebExceptionWrapper(webException);
                _innerException = webException;
            }
            catch (WebExceptionWrapper webException)
            {
                webExceptionWrapper = webException;
                _innerException = webException;
            }
            catch (Exception ex)
            {
                webExceptionWrapper = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                _innerException = webExceptionWrapper;
            }
            finally
            {
                if (webExceptionWrapper != null)
                    OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(null, webExceptionWrapper, webExceptionWrapper.Status == WebExceptionStatus.RequestCanceled, userToken));
            }
        }

        public virtual void OpenWriteAsync()
        {
            OpenWriteAsync(null);
        }

        private void ResponseCallback(IAsyncResult asyncResult, object userToken)
        {
            WebExceptionWrapper webExceptionWrapper = null;
            Stream stream = null;
            try
            {
                if (_httpWebResponse == null)
                    _httpWebResponse = _httpWebRequest.EndGetResponse(asyncResult);
                stream = _httpWebResponse.GetResponseStream();
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                    _httpWebResponse = new HttpWebResponseWrapper((HttpWebResponse)webException.Response);
                webExceptionWrapper = new WebExceptionWrapper(webException);
                _innerException = webExceptionWrapper;
            }
            catch (WebExceptionWrapper webException)
            {
                _httpWebResponse = webException.GetResponse();
                webExceptionWrapper = webException;
                _innerException = webExceptionWrapper;
            }
            catch (Exception ex)
            {
                webExceptionWrapper = new WebExceptionWrapper(new WebException("An error occurred performing a http web request.", ex));
                _innerException = webExceptionWrapper;
            }

            OnOpenReadCompleted(new OpenReadCompletedEventArgs(stream, webExceptionWrapper, webExceptionWrapper != null && webExceptionWrapper.Status == WebExceptionStatus.RequestCanceled, userToken));
        }

        private void ScanTimoutCallback(object state, bool timedOut)
        {
            if (HttpWebRequest != null && timedOut)
                HttpWebRequest.Abort();
        }

#if FLUENTHTTP_CORE_TPL

        static void TransferCompletionToTask<T>(TaskCompletionSource<T> tcs, AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
        {
            if (e.UserState != tcs)
                return;

            try
            {
                unregisterHandler();
            }
            finally
            {
                if (e.Cancelled) tcs.TrySetCanceled();
                else if (e.Error != null) tcs.TrySetException(e.Error);
                else tcs.TrySetResult(getResult());
            }
        }

        public virtual Task<Stream> OpenReadTaskAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Stream>(this);

            var ctr = cancellationToken.Register(CancelAsync);
            OpenReadCompletedEventHandler handler = null;
            handler = (sender, e) => TransferCompletionToTask(tcs, e, () => e.Result, () => { ctr.Dispose(); OpenReadCompleted -= handler; });
            OpenReadCompleted += handler;

            try
            {
                OpenReadAsync(tcs);
            }
            catch
            {
                OpenReadCompleted -= handler;
                throw;
            }

            return tcs.Task;
        }

        public virtual Task<Stream> OpenReadTaskAsync()
        {
            return OpenReadTaskAsync(CancellationToken.None);
        }

        public virtual Task<Stream> OpenWriteTaskAsync()
        {
            return OpenWriteTaskAsync(CancellationToken.None);
        }

        public virtual Task<Stream> OpenWriteTaskAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<Stream>(this);

            var ctr = cancellationToken.Register(CancelAsync);
            OpenWriteCompletedEventHandler handler = null;
            handler = (sender, e) => TransferCompletionToTask(tcs, e, () => e.Result, () => { ctr.Dispose(); OpenWriteCompleted -= handler; });
            OpenWriteCompleted += handler;

            try
            {
                OpenWriteAsync(tcs);
            }
            catch
            {
                OpenWriteCompleted -= handler;
                throw;
            }

            return tcs.Task;
        }

#endif

        public void CancelAsync()
        {
            HttpWebRequest.Abort();
        }

        protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs args)
        {
            if (OpenReadCompleted != null)
                OpenReadCompleted(this, args);
        }

        protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs args)
        {
            if (OpenWriteCompleted != null)
                OpenWriteCompleted(this, args);
        }

        #region UrlEncoding/UrlDecoding

#if FLUENTHTTP_URLENCODING

        public static string UrlEncode(string s)
        {
            return Uri.EscapeDataString(s);
        }

        public static string UrlDecode(string s)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlDecode(s);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlDecode(s);
#else
            return UrlDecode(s, Encoding.UTF8);
#endif
        }

#if !(SILVERLIGHT || WINDOWS_PHONE)
        private static string UrlDecode(string s, Encoding e)
        {
            if (null == s)
                return null;

            if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
                return s;

            if (e == null)
                e = Encoding.UTF8;

            long len = s.Length;
            var bytes = new List<byte>();
            int xchar;
            char ch;

            for (int i = 0; i < len; i++)
            {
                ch = s[i];
                if (ch == '%' && i + 2 < len && s[i + 1] != '%')
                {
                    if (s[i + 1] == 'u' && i + 5 < len)
                    {
                        // unicode hex sequence
                        xchar = GetChar(s, i + 2, 4);
                        if (xchar != -1)
                        {
                            WriteCharBytes(bytes, (char)xchar, e);
                            i += 5;
                        }
                        else
                            WriteCharBytes(bytes, '%', e);
                    }
                    else if ((xchar = GetChar(s, i + 1, 2)) != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, e);
                        i += 2;
                    }
                    else
                    {
                        WriteCharBytes(bytes, '%', e);
                    }
                    continue;
                }

                if (ch == '+')
                    WriteCharBytes(bytes, ' ', e);
                else
                    WriteCharBytes(bytes, ch, e);
            }

            byte[] buf = bytes.ToArray();
            bytes = null;
            return e.GetString(buf, 0, buf.Length);

        }

        private static void WriteCharBytes(IList buf, char ch, Encoding e)
        {
            if (ch > 255)
            {
                foreach (byte b in e.GetBytes(new char[] { ch }))
                    buf.Add(b);
            }
            else
                buf.Add((byte)ch);
        }

        private static int GetChar(string str, int offset, int length)
        {
            int val = 0;
            int end = length + offset;
            for (int i = offset; i < end; i++)
            {
                char c = str[i];
                if (c > 127)
                    return -1;

                int current = GetInt((byte)c);
                if (current == -1)
                    return -1;
                val = (val << 4) + current;
            }

            return val;
        }

        private static int GetInt(byte b)
        {
            char c = (char)b;
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            return -1;
        }
#endif

#endif

        #endregion

        #region HtmlDecoding

#if FLUENTHTTP_HTMLENCODING

#if !(SILVERLIGHT || WINDOWS_PHONE)

        public static readonly System.Globalization.CultureInfo InvariantCulture =
            System.Globalization.CultureInfo.InvariantCulture;

        private static object entitiesLock = new object();
        private static SortedDictionary<string, char> entities;

        private static void InitEntities()
        {
            // Build the hash table of HTML entity references.  This list comes
            // from the HTML 4.01 W3C recommendation.
            entities = new SortedDictionary<string, char>(StringComparer.Ordinal);

            entities.Add("nbsp", '\u00A0');
            entities.Add("iexcl", '\u00A1');
            entities.Add("cent", '\u00A2');
            entities.Add("pound", '\u00A3');
            entities.Add("curren", '\u00A4');
            entities.Add("yen", '\u00A5');
            entities.Add("brvbar", '\u00A6');
            entities.Add("sect", '\u00A7');
            entities.Add("uml", '\u00A8');
            entities.Add("copy", '\u00A9');
            entities.Add("ordf", '\u00AA');
            entities.Add("laquo", '\u00AB');
            entities.Add("not", '\u00AC');
            entities.Add("shy", '\u00AD');
            entities.Add("reg", '\u00AE');
            entities.Add("macr", '\u00AF');
            entities.Add("deg", '\u00B0');
            entities.Add("plusmn", '\u00B1');
            entities.Add("sup2", '\u00B2');
            entities.Add("sup3", '\u00B3');
            entities.Add("acute", '\u00B4');
            entities.Add("micro", '\u00B5');
            entities.Add("para", '\u00B6');
            entities.Add("middot", '\u00B7');
            entities.Add("cedil", '\u00B8');
            entities.Add("sup1", '\u00B9');
            entities.Add("ordm", '\u00BA');
            entities.Add("raquo", '\u00BB');
            entities.Add("frac14", '\u00BC');
            entities.Add("frac12", '\u00BD');
            entities.Add("frac34", '\u00BE');
            entities.Add("iquest", '\u00BF');
            entities.Add("Agrave", '\u00C0');
            entities.Add("Aacute", '\u00C1');
            entities.Add("Acirc", '\u00C2');
            entities.Add("Atilde", '\u00C3');
            entities.Add("Auml", '\u00C4');
            entities.Add("Aring", '\u00C5');
            entities.Add("AElig", '\u00C6');
            entities.Add("Ccedil", '\u00C7');
            entities.Add("Egrave", '\u00C8');
            entities.Add("Eacute", '\u00C9');
            entities.Add("Ecirc", '\u00CA');
            entities.Add("Euml", '\u00CB');
            entities.Add("Igrave", '\u00CC');
            entities.Add("Iacute", '\u00CD');
            entities.Add("Icirc", '\u00CE');
            entities.Add("Iuml", '\u00CF');
            entities.Add("ETH", '\u00D0');
            entities.Add("Ntilde", '\u00D1');
            entities.Add("Ograve", '\u00D2');
            entities.Add("Oacute", '\u00D3');
            entities.Add("Ocirc", '\u00D4');
            entities.Add("Otilde", '\u00D5');
            entities.Add("Ouml", '\u00D6');
            entities.Add("times", '\u00D7');
            entities.Add("Oslash", '\u00D8');
            entities.Add("Ugrave", '\u00D9');
            entities.Add("Uacute", '\u00DA');
            entities.Add("Ucirc", '\u00DB');
            entities.Add("Uuml", '\u00DC');
            entities.Add("Yacute", '\u00DD');
            entities.Add("THORN", '\u00DE');
            entities.Add("szlig", '\u00DF');
            entities.Add("agrave", '\u00E0');
            entities.Add("aacute", '\u00E1');
            entities.Add("acirc", '\u00E2');
            entities.Add("atilde", '\u00E3');
            entities.Add("auml", '\u00E4');
            entities.Add("aring", '\u00E5');
            entities.Add("aelig", '\u00E6');
            entities.Add("ccedil", '\u00E7');
            entities.Add("egrave", '\u00E8');
            entities.Add("eacute", '\u00E9');
            entities.Add("ecirc", '\u00EA');
            entities.Add("euml", '\u00EB');
            entities.Add("igrave", '\u00EC');
            entities.Add("iacute", '\u00ED');
            entities.Add("icirc", '\u00EE');
            entities.Add("iuml", '\u00EF');
            entities.Add("eth", '\u00F0');
            entities.Add("ntilde", '\u00F1');
            entities.Add("ograve", '\u00F2');
            entities.Add("oacute", '\u00F3');
            entities.Add("ocirc", '\u00F4');
            entities.Add("otilde", '\u00F5');
            entities.Add("ouml", '\u00F6');
            entities.Add("divide", '\u00F7');
            entities.Add("oslash", '\u00F8');
            entities.Add("ugrave", '\u00F9');
            entities.Add("uacute", '\u00FA');
            entities.Add("ucirc", '\u00FB');
            entities.Add("uuml", '\u00FC');
            entities.Add("yacute", '\u00FD');
            entities.Add("thorn", '\u00FE');
            entities.Add("yuml", '\u00FF');
            entities.Add("fnof", '\u0192');
            entities.Add("Alpha", '\u0391');
            entities.Add("Beta", '\u0392');
            entities.Add("Gamma", '\u0393');
            entities.Add("Delta", '\u0394');
            entities.Add("Epsilon", '\u0395');
            entities.Add("Zeta", '\u0396');
            entities.Add("Eta", '\u0397');
            entities.Add("Theta", '\u0398');
            entities.Add("Iota", '\u0399');
            entities.Add("Kappa", '\u039A');
            entities.Add("Lambda", '\u039B');
            entities.Add("Mu", '\u039C');
            entities.Add("Nu", '\u039D');
            entities.Add("Xi", '\u039E');
            entities.Add("Omicron", '\u039F');
            entities.Add("Pi", '\u03A0');
            entities.Add("Rho", '\u03A1');
            entities.Add("Sigma", '\u03A3');
            entities.Add("Tau", '\u03A4');
            entities.Add("Upsilon", '\u03A5');
            entities.Add("Phi", '\u03A6');
            entities.Add("Chi", '\u03A7');
            entities.Add("Psi", '\u03A8');
            entities.Add("Omega", '\u03A9');
            entities.Add("alpha", '\u03B1');
            entities.Add("beta", '\u03B2');
            entities.Add("gamma", '\u03B3');
            entities.Add("delta", '\u03B4');
            entities.Add("epsilon", '\u03B5');
            entities.Add("zeta", '\u03B6');
            entities.Add("eta", '\u03B7');
            entities.Add("theta", '\u03B8');
            entities.Add("iota", '\u03B9');
            entities.Add("kappa", '\u03BA');
            entities.Add("lambda", '\u03BB');
            entities.Add("mu", '\u03BC');
            entities.Add("nu", '\u03BD');
            entities.Add("xi", '\u03BE');
            entities.Add("omicron", '\u03BF');
            entities.Add("pi", '\u03C0');
            entities.Add("rho", '\u03C1');
            entities.Add("sigmaf", '\u03C2');
            entities.Add("sigma", '\u03C3');
            entities.Add("tau", '\u03C4');
            entities.Add("upsilon", '\u03C5');
            entities.Add("phi", '\u03C6');
            entities.Add("chi", '\u03C7');
            entities.Add("psi", '\u03C8');
            entities.Add("omega", '\u03C9');
            entities.Add("thetasym", '\u03D1');
            entities.Add("upsih", '\u03D2');
            entities.Add("piv", '\u03D6');
            entities.Add("bull", '\u2022');
            entities.Add("hellip", '\u2026');
            entities.Add("prime", '\u2032');
            entities.Add("Prime", '\u2033');
            entities.Add("oline", '\u203E');
            entities.Add("frasl", '\u2044');
            entities.Add("weierp", '\u2118');
            entities.Add("image", '\u2111');
            entities.Add("real", '\u211C');
            entities.Add("trade", '\u2122');
            entities.Add("alefsym", '\u2135');
            entities.Add("larr", '\u2190');
            entities.Add("uarr", '\u2191');
            entities.Add("rarr", '\u2192');
            entities.Add("darr", '\u2193');
            entities.Add("harr", '\u2194');
            entities.Add("crarr", '\u21B5');
            entities.Add("lArr", '\u21D0');
            entities.Add("uArr", '\u21D1');
            entities.Add("rArr", '\u21D2');
            entities.Add("dArr", '\u21D3');
            entities.Add("hArr", '\u21D4');
            entities.Add("forall", '\u2200');
            entities.Add("part", '\u2202');
            entities.Add("exist", '\u2203');
            entities.Add("empty", '\u2205');
            entities.Add("nabla", '\u2207');
            entities.Add("isin", '\u2208');
            entities.Add("notin", '\u2209');
            entities.Add("ni", '\u220B');
            entities.Add("prod", '\u220F');
            entities.Add("sum", '\u2211');
            entities.Add("minus", '\u2212');
            entities.Add("lowast", '\u2217');
            entities.Add("radic", '\u221A');
            entities.Add("prop", '\u221D');
            entities.Add("infin", '\u221E');
            entities.Add("ang", '\u2220');
            entities.Add("and", '\u2227');
            entities.Add("or", '\u2228');
            entities.Add("cap", '\u2229');
            entities.Add("cup", '\u222A');
            entities.Add("int", '\u222B');
            entities.Add("there4", '\u2234');
            entities.Add("sim", '\u223C');
            entities.Add("cong", '\u2245');
            entities.Add("asymp", '\u2248');
            entities.Add("ne", '\u2260');
            entities.Add("equiv", '\u2261');
            entities.Add("le", '\u2264');
            entities.Add("ge", '\u2265');
            entities.Add("sub", '\u2282');
            entities.Add("sup", '\u2283');
            entities.Add("nsub", '\u2284');
            entities.Add("sube", '\u2286');
            entities.Add("supe", '\u2287');
            entities.Add("oplus", '\u2295');
            entities.Add("otimes", '\u2297');
            entities.Add("perp", '\u22A5');
            entities.Add("sdot", '\u22C5');
            entities.Add("lceil", '\u2308');
            entities.Add("rceil", '\u2309');
            entities.Add("lfloor", '\u230A');
            entities.Add("rfloor", '\u230B');
            entities.Add("lang", '\u2329');
            entities.Add("rang", '\u232A');
            entities.Add("loz", '\u25CA');
            entities.Add("spades", '\u2660');
            entities.Add("clubs", '\u2663');
            entities.Add("hearts", '\u2665');
            entities.Add("diams", '\u2666');
            entities.Add("quot", '\u0022');
            entities.Add("amp", '\u0026');
            entities.Add("lt", '\u003C');
            entities.Add("gt", '\u003E');
            entities.Add("OElig", '\u0152');
            entities.Add("oelig", '\u0153');
            entities.Add("Scaron", '\u0160');
            entities.Add("scaron", '\u0161');
            entities.Add("Yuml", '\u0178');
            entities.Add("circ", '\u02C6');
            entities.Add("tilde", '\u02DC');
            entities.Add("ensp", '\u2002');
            entities.Add("emsp", '\u2003');
            entities.Add("thinsp", '\u2009');
            entities.Add("zwnj", '\u200C');
            entities.Add("zwj", '\u200D');
            entities.Add("lrm", '\u200E');
            entities.Add("rlm", '\u200F');
            entities.Add("ndash", '\u2013');
            entities.Add("mdash", '\u2014');
            entities.Add("lsquo", '\u2018');
            entities.Add("rsquo", '\u2019');
            entities.Add("sbquo", '\u201A');
            entities.Add("ldquo", '\u201C');
            entities.Add("rdquo", '\u201D');
            entities.Add("bdquo", '\u201E');
            entities.Add("dagger", '\u2020');
            entities.Add("Dagger", '\u2021');
            entities.Add("permil", '\u2030');
            entities.Add("lsaquo", '\u2039');
            entities.Add("rsaquo", '\u203A');
            entities.Add("euro", '\u20AC');
        }

        private static IDictionary<string, char> Entities
        {
            get
            {
                lock (entitiesLock)
                {
                    if (entities == null)
                        InitEntities();

                    return entities;
                }
            }
        }

#endif

        public static string HtmlDecode(string s)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.HtmlDecode(s);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.HtmlDecode(s);
#else
            if (s == null)
                return null;

            if (s.Length == 0)
                return String.Empty;

            if (s.IndexOf('&') == -1)
                return s;
#if NET_4_0
			StringBuilder rawEntity = new StringBuilder ();
#endif
            StringBuilder entity = new StringBuilder();
            StringBuilder output = new StringBuilder();
            int len = s.Length;
            // 0 -> nothing,
            // 1 -> right after '&'
            // 2 -> between '&' and ';' but no '#'
            // 3 -> '#' found after '&' and getting numbers
            int state = 0;
            int number = 0;
            bool is_hex_value = false;
            bool have_trailing_digits = false;

            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if (state == 0)
                {
                    if (c == '&')
                    {
                        entity.Append(c);
#if NET_4_0
						rawEntity.Append (c);
#endif
                        state = 1;
                    }
                    else
                    {
                        output.Append(c);
                    }
                    continue;
                }

                if (c == '&')
                {
                    state = 1;
                    if (have_trailing_digits)
                    {
                        entity.Append(number.ToString(InvariantCulture));
                        have_trailing_digits = false;
                    }

                    output.Append(entity.ToString());
                    entity.Length = 0;
                    entity.Append('&');
                    continue;
                }

                if (state == 1)
                {
                    if (c == ';')
                    {
                        state = 0;
                        output.Append(entity.ToString());
                        output.Append(c);
                        entity.Length = 0;
                    }
                    else
                    {
                        number = 0;
                        is_hex_value = false;
                        if (c != '#')
                        {
                            state = 2;
                        }
                        else
                        {
                            state = 3;
                        }
                        entity.Append(c);
#if NET_4_0
						rawEntity.Append (c);
#endif
                    }
                }
                else if (state == 2)
                {
                    entity.Append(c);
                    if (c == ';')
                    {
                        string key = entity.ToString();
                        if (key.Length > 1 && Entities.ContainsKey(key.Substring(1, key.Length - 2)))
                            key = Entities[key.Substring(1, key.Length - 2)].ToString();

                        output.Append(key);
                        state = 0;
                        entity.Length = 0;
#if NET_4_0
						rawEntity.Length = 0;
#endif
                    }
                }
                else if (state == 3)
                {
                    if (c == ';')
                    {
#if NET_4_0
						if (number == 0)
							output.Append (rawEntity.ToString () + ";");
						else
#endif
                        if (number > 65535)
                        {
                            output.Append("&#");
                            output.Append(number.ToString(InvariantCulture));
                            output.Append(";");
                        }
                        else
                        {
                            output.Append((char)number);
                        }
                        state = 0;
                        entity.Length = 0;
#if NET_4_0
						rawEntity.Length = 0;
#endif
                        have_trailing_digits = false;
                    }
#if FLUENTHTTP_CORE_WINRT
                    else if (is_hex_value && IsHexDigit(c))
#else
                    else if (is_hex_value && Uri.IsHexDigit(c))
#endif
                    {
#if FLUENTHTTP_CORE_WINRT
                        number = number * 16 + FromHex(c);
#else
                        number = number * 16 + Uri.FromHex(c);
#endif
                        have_trailing_digits = true;
#if NET_4_0
						rawEntity.Append (c);
#endif
                    }
                    else if (Char.IsDigit(c))
                    {
                        number = number * 10 + ((int)c - '0');
                        have_trailing_digits = true;
#if NET_4_0
						rawEntity.Append (c);
#endif
                    }
                    else if (number == 0 && (c == 'x' || c == 'X'))
                    {
                        is_hex_value = true;
#if NET_4_0
						rawEntity.Append (c);
#endif
                    }
                    else
                    {
                        state = 2;
                        if (have_trailing_digits)
                        {
                            entity.Append(number.ToString(InvariantCulture));
                            have_trailing_digits = false;
                        }
                        entity.Append(c);
                    }
                }
            }

            if (entity.Length > 0)
            {
                output.Append(entity.ToString());
            }
            else if (have_trailing_digits)
            {
                output.Append(number.ToString(InvariantCulture));
            }
            return output.ToString();
#endif
        }

#if FLUENTHTTP_CORE_WINRT
        private static bool IsHexDigit(char digit)
        {
            return (('0' <= digit && digit <= '9') ||
                    ('a' <= digit && digit <= 'f') ||
                    ('A' <= digit && digit <= 'F'));
        }

        private static int FromHex(char digit)
        {
            if ('0' <= digit && digit <= '9')
            {
                return (int)(digit - '0');
            }

            if ('a' <= digit && digit <= 'f')
                return (int)(digit - 'a' + 10);

            if ('A' <= digit && digit <= 'F')
                return (int)(digit - 'A' + 10);

            throw new ArgumentException("digit");
        }
#endif

#endif

        #endregion

        #region Utils

#if FLUENTHTTP_CORE_UTILS

        public static string BuildRequestUrl<TKey, TValue>(string baseUrl, string resourcePath, IEnumerable<KeyValuePair<TKey, TValue>> queryStrings)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException("baseUrl");

            sb.Append(baseUrl);
            if (!string.IsNullOrEmpty(resourcePath))
                sb.Append(AddStartingSlashIfNotPresent(resourcePath));
            sb.Append("?");

            if (queryStrings != null)
            {
                foreach (var queryString in queryStrings)
                {
                    // note: assume queryString is already url encoded.
                    sb.AppendFormat("{0}={1}&", queryString.Key, queryString.Value);
                }
            }

            // remote the last & or ?
            --sb.Length;

            return sb.ToString();
        }

        public static string AddStartingSlashIfNotPresent(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "/";
            }

            // if not null or empty
            if (input[0] != '/')
            {
                // if doesn't start with / then add /
                return "/" + input;
            }
            else
            {
                // else return the original input.
                return input;
            }
        }

        public virtual void SetHeaders<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> requestHeaders)
            where TKey : class
            where TValue : class
        {
            foreach (var requestHeader in requestHeaders)
            {
                if (requestHeader.Key == null)
                    throw new ArgumentNullException("key");
                if (requestHeader.Value == null)
                    throw new ArgumentNullException("value");

                string name = requestHeader.Key.ToString();
                string value = requestHeader.Value.ToString();

                // todo: add more special headers
                if (name.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                {
                    _httpWebRequest.ContentType = value;
                }
                else if (name.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                {
#if (WINDOWS_PHONE ||FLUENTHTTP_CORE_WINRT)
                    throw new Exception("Cannot set content-length.");
#else
                    _httpWebRequest.ContentLength = Convert.ToInt64(value);
#endif
                }
                else if (name.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                {
#if FLUENTHTTP_CORE_WINRT
                    throw new Exception("Cannot set user-agent.");
#else
                    _httpWebRequest.UserAgent = value;
#endif
                }
                else
                {
                    _httpWebRequest.Headers[name] = value;
                }
            }
        }

#endif

#if FLUENTHTTP_CORE_STREAM

        public void CopyStream(Stream input, Stream output, int? bufferSize, bool flushInput, bool flushOutput)
        {
            byte[] buffer = new byte[bufferSize ?? 1024 * 4]; // 4 kb
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (flushInput)
                    input.Flush();
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
                if (flushOutput)
                    output.Flush();
            }
        }

        public void ReadStream(Stream input, int? bufferSize, bool flushInput)
        {
            byte[] buffer = new byte[bufferSize ?? 1024 * 4]; // 4 kb
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (flushInput)
                    input.Flush();
                if (read <= 0)
                    return;
            }
        }

#if FLUENTHTTP_CORE_TPL

        public static System.Threading.Tasks.Task<int> ReadTask(Stream stream, byte[] buffer, int offset, int count)
        {
            return System.Threading.Tasks.Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, offset, count, null, System.Threading.Tasks.TaskCreationOptions.None);
        }

        public static System.Threading.Tasks.Task WriteTask(Stream stream, byte[] buffer, int offset, int count)
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, buffer, offset, count, null, System.Threading.Tasks.TaskCreationOptions.None);
        }

#endif

#endif

        #endregion

        #region Authentication

#if FLUENTHTTP_HTTPBASIC_AUTHENTICATION

        public void AuthenticateUsingHttpBasicAuthentication(string username, string password)
        {
            _httpWebRequest.Headers["Authorization"] = string.Concat("Basic ", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password))));
        }

#endif
        #endregion

    }
}
