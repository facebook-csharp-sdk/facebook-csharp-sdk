namespace Facebook.Tests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class FakeHttpWebRequestWrapper : HttpWebRequestWrapper
    {
        private Uri _uri;
        private Func<HttpWebResponseWrapper> _getResponse;
        private Func<Stream> _getRequestStream;
        private WebHeaderCollection _headers;

        public override Uri RequestUri { get { return _uri; } }
        public override string Method { get; set; }
        public override string ContentType { get; set; }
        public override WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

#if !(WINDOWS_PHONE || NETFX_CORE)
        public override long ContentLength { get; set; }
        public override bool AllowWriteStreamBuffering { get; set; }
#endif

#if !(SILVERLIGHT || NETFX_CORE)
        public override DecompressionMethods AutomaticDecompression { get; set; }
#endif

#if !(NETFX_CORE)
        public override string UserAgent { get; set; }
#endif

        public FakeHttpWebRequestWrapper()
        {
            Headers = new WebHeaderCollection();
        }

        public override HttpWebResponseWrapper GetResponse()
        {
            return _getResponse();
        }

        public override Stream GetRequestStream()
        {
            return _getRequestStream();
        }

        public FakeHttpWebRequestWrapper WithRequestUri(Uri uri)
        {
            _uri = uri;
            return this;
        }

        public FakeHttpWebResponseWrapper FakeResponse()
        {
            var respone = new FakeHttpWebResponseWrapper(this);
            _getResponse = () => respone;
            return respone;
        }

        public FakeHttpWebRequestWrapper OnGeResponse(Func<FakeHttpWebResponseWrapper> onGetResponse)
        {
            _getResponse = onGetResponse;
            return this;
        }

        public FakeHttpWebResponseWrapper FakeResponse(out FakeHttpWebResponseWrapper fakeResponse)
        {
            return fakeResponse = FakeResponse();
        }
    }

    public class FakeHttpWebResponseWrapper : HttpWebResponseWrapper
    {
        private readonly FakeHttpWebRequestWrapper _httpWebRequestWrapper;
        private string _contentType;
        private long _contentLength;
        private HttpStatusCode _statusCode;
        private Uri _responseUri;
        private Func<Stream> _getResponseStream;
        private WebHeaderCollection _headers;

        public FakeHttpWebResponseWrapper(FakeHttpWebRequestWrapper httpWebRequestWrapper)
        {
            _httpWebRequestWrapper = httpWebRequestWrapper;
            _responseUri = httpWebRequestWrapper.RequestUri;
            _headers = new WebHeaderCollection();
        }

        public override long ContentLength { get { return _contentLength; } }
        public override string ContentType { get { return _contentType; } }
        public override Uri ResponseUri { get { return _responseUri; } }
        public override HttpStatusCode StatusCode { get { return _statusCode; } }
        public override WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        public override Stream GetResponseStream()
        {
            return _getResponseStream();
        }

        public FakeHttpWebResponseWrapper WithContentType(string contentType)
        {
            _contentType = contentType;
            Headers["Content-Type"] = contentType;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(Func<Stream> getResponseStream, long contentLength)
        {
            _getResponseStream = getResponseStream;
            _contentLength = contentLength;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(Func<Stream> getResponseStream)
        {
            _getResponseStream = getResponseStream;
            _contentLength = getResponseStream().Length;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(string contents, Encoding encoding)
        {
            return WithResponseStreamAs(() => new MemoryStream(encoding.GetBytes(contents)));
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(string contents)
        {
            return WithResponseStreamAs(contents, Encoding.UTF8);
        }

        public FakeHttpWebResponseWrapper WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            return this;
        }

        public FakeHttpWebResponseWrapper WithStatusCode(int statusCode)
        {
            return WithStatusCode((HttpStatusCode)statusCode);
        }

        public FakeHttpWebResponseWrapper WithResponseUri(Uri uri)
        {
            _responseUri = uri;
            return this;
        }

        public FakeHttpWebResponseWrapper WithHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }

        public FakeHttpWebRequestWrapper GetFakeHttpWebRequestWrapper()
        {
            return _httpWebRequestWrapper;
        }

        public FakeHttpWebRequestWrapper As304NotModified(Action<FakeHttpWebResponseWrapper> responseCallback)
        {
            return GetFakeHttpWebRequestWrapper().OnGeResponse(() =>
                                                                {
                                                                    var res = new FakeHttpWebResponseWrapper(GetFakeHttpWebRequestWrapper())
                                                                        .WithStatusCode(HttpStatusCode.NotModified)
                                                                        .WithResponseStreamAs(() => new MemoryStream());

                                                                    responseCallback(res);

                                                                    throw new FakeWebExceptionWrapper()
                                                                        .WithResponse(() => res)
                                                                        .WithStatus(WebExceptionStatus.ProtocolError);
                                                                });
        }
    }

    public class FakeWebExceptionWrapper : WebExceptionWrapper
    {
        private Func<HttpWebResponseWrapper> _getResponse;
        private WebExceptionStatus _status;

        public FakeWebExceptionWrapper()
        {
        }

        public FakeWebExceptionWrapper(string message)
        {
            throw new NotImplementedException();
        }

        public FakeWebExceptionWrapper(string message, Exception innerException)
        {
            throw new NotImplementedException();
        }

        public override HttpWebResponseWrapper GetResponse()
        {
            return _getResponse();
        }

        public override WebExceptionStatus Status
        {
            get { return _status; }
        }

        public FakeWebExceptionWrapper WithResponse(Func<FakeHttpWebResponseWrapper> getResponse)
        {
            _getResponse = getResponse;
            return this;
        }

        public FakeWebExceptionWrapper WithStatus(WebExceptionStatus status)
        {
            _status = status;
            return this;
        }
    }
}