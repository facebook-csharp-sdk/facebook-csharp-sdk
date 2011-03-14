
namespace Facebook.Web
{
    /// <summary>
    /// Represents an incoming FacebookCanvasRequest.
    /// </summary>
    public class CanvasContext : FacebookWebContext
    {
        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession _session;

        /// <summary>
        /// The facebook signed request.
        /// </summary>
        private FacebookSignedRequest _signedRequest;

        public new static CanvasContext Current
        {
            get { return new CanvasContext(); }
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public override FacebookSession Session
        {
            get
            {
                return _session ??
                       (_session = FacebookSession.GetSession(Settings.AppId, Settings.AppSecret, HttpContext, SignedRequest));
            }
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                return _signedRequest ??
                    (_signedRequest = FacebookSignedRequest.GetSignedRequest(Settings.AppSecret, HttpContext));
            }
        }

    }
}
