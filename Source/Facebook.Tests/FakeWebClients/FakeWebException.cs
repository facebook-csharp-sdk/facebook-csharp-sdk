
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

        public override bool HasResponse
        {
            get { return true; }
        }

        public override Stream GetResponseStream()
        {
            return this.responseStream;
        }
    }
}