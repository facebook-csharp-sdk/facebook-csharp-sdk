namespace Facebook.Web
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Facebook helper methods for web.
    /// </summary>
    internal class FacebookWebUtils
    {
        /// <summary>
        /// Gets the facebook signed request from the http request.
        /// </summary>
        /// <param name="appSecret">
        /// The app Secret.
        /// </param>
        /// <param name="httpRequest">
        /// The http request.
        /// </param>
        /// <returns>
        /// Returns the signed request if found otherwise null.
        /// </returns>
        public static FacebookSignedRequest GetSignedRequest(string appSecret, HttpRequestBase httpRequest)
        {
            Contract.Requires(httpRequest != null);
            Contract.Requires(httpRequest.Params != null);

            return httpRequest.Params.AllKeys.Contains("signed_request") ? FacebookSignedRequest.Parse(appSecret, httpRequest.Params["signed_request"]) : null;
        }
    }
}