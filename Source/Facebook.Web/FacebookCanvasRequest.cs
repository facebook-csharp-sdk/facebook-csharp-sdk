using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    public class FacebookCanvasRequest : FacebookWebRequest
    {

        /// <summary>
        /// The facebook session.
        /// </summary>
        private FacebookSession m_session;

        /// <summary>
        /// The facebook signed request.
        /// </summary>
        private FacebookSignedRequest m_signedRequest;

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
