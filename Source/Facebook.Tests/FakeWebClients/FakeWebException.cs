
namespace Facebook.Tests.FakeWebClients
{
    using System.IO;
    using System.Text;
    using Facebook;

    /// <summary>
    /// </summary>
    internal class FakeWebException : WebExceptionWrapper
    {
        private readonly Stream responseStream;

        public FakeWebException(Stream responseStream)
            : base(null)
        {
            this.responseStream = responseStream;
        }

        public FakeWebException(string json)
            : this(new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
        }

        public override HttpWebResponseWrapper GetResponse()
        {
            return new FakeHttpWebResponse(responseStream);
        }

        class FakeHttpWebResponse : HttpWebResponseWrapper
        {
            private readonly Stream _stream;

            public FakeHttpWebResponse(Stream stream)
            {
                _stream = stream;
            }

            public override Stream GetResponseStream()
            {
                return _stream;
            }
        }
    }
}