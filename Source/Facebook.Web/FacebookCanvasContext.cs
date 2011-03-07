using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    /// <summary>
    /// Represents an incoming FacebookCanvasRequest.
    /// </summary>
    public class FacebookCanvasContext : FacebookWebContext
    {

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession m_session;

        /// <summary>
        /// The facebook signed request.
        /// </summary>
        private FacebookSignedRequest m_signedRequest;

        public new static FacebookCanvasContext Current
        {
            get { return new FacebookCanvasContext(); }
        }

        /// <summary>
        /// Gets the facebook session.
        /// </summary>
        public override FacebookSession Session
        {
            get
            {
                return this.m_session ??
                       (this.m_session = FacebookSession.GetSession(this.Settings.AppId, this.Settings.AppSecret, this.HttpContext, this.SignedRequest));
            }
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                return this.m_signedRequest ??
                    (this.m_signedRequest = FacebookSignedRequest.GetSignedRequest(this.Settings.AppSecret, this.HttpContext));
            }
        }

    }
}
