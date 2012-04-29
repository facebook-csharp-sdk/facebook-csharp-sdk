namespace Facebook.Tests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class FakeHttpWebRequestWrapper : HttpWebRequestWrapper
    {
        private Uri _uri;
        private Func<Stream> _requestStream;
        private HttpWebResponseWrapper _httpWebResponseWrapper;
        public override Uri RequestUri { get { return _uri; } }
        public override string Method { get; set; }
        public override string ContentType { get; set; }

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

        public override HttpWebResponseWrapper GetResponse()
        {
            return _httpWebResponseWrapper;
        }

        public override Stream GetRequestStream()
        {
            return _requestStream();
        }

        public FakeHttpWebRequestWrapper WithRequestUri(Uri uri)
        {
            _uri = uri;
            return this;
        }

        public FakeHttpWebResponseWrapper FakeResponse()
        {
            _httpWebResponseWrapper = new FakeHttpWebResponseWrapper(this);
            return _httpWebResponseWrapper as FakeHttpWebResponseWrapper;
        }
    }

    public class FakeHttpWebResponseWrapper : HttpWebResponseWrapper
    {
        private HttpWebRequestWrapper _httpWebRequestWrapper;
        private Func<Stream> _responseStream;
        private string _contentType;
        private long _contentLength;
        private HttpStatusCode _statusCode;
        private Uri _responseUri;

        public FakeHttpWebResponseWrapper()
        {
        }

        public FakeHttpWebResponseWrapper(HttpWebRequestWrapper httpWebRequestWrapper)
        {
            _httpWebRequestWrapper = httpWebRequestWrapper;
            _responseUri = _httpWebRequestWrapper.RequestUri;
        }

        public override Stream GetResponseStream()
        {
            return _responseStream();
        }

        public override long ContentLength { get { return _contentLength; } }
        public override string ContentType { get { return _contentType; } }
        public override Uri ResponseUri { get { return _responseUri; } }
        public override HttpStatusCode StatusCode { get { return _statusCode; } }

        public FakeHttpWebResponseWrapper WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(string contents)
        {
            return WithResponseStreamAs(contents, Encoding.UTF8);
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(string contents, Encoding encoding)
        {
            var stream = new MemoryStream(encoding.GetBytes(contents));
            _responseStream = () => stream;
            _contentLength = stream.Length;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseStreamAs(Func<Stream> responseStream)
        {
            _responseStream = responseStream;
            _contentLength = _responseStream().Length;
            return this;
        }

        public FakeHttpWebResponseWrapper WithStatusCode(int statusCode)
        {
            return WithStatusCode((HttpStatusCode)statusCode);
        }

        public FakeHttpWebResponseWrapper WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            return this;
        }

        public FakeHttpWebResponseWrapper WithResponseUri(Uri uri)
        {
            _responseUri = uri;
            return this;
        }

        public FakeHttpWebRequestWrapper GetFakeHttpWebRequestWrapper()
        {
            return _httpWebRequestWrapper as FakeHttpWebRequestWrapper;
        }
    }
}