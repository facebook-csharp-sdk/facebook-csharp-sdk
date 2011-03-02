namespace Facebook
{
    using System;
    using System.Diagnostics.Contracts;

    public partial class FacebookClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/>.
        /// </summary>
        /// <param name="appId">The Facebook application id.</param>
        /// <param name="appSecret">The Facebook application secret.</param>
        [Obsolete("Method marked for removal.")]
        public FacebookClient(string appId, string appSecret)
        {
            Contract.Requires(!String.IsNullOrEmpty(appId));
            Contract.Requires(!String.IsNullOrEmpty(appSecret));

            this.AccessToken = String.Concat(appId, "|", appSecret);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookClient"/> class.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        [Obsolete("Method marked for removal.")]
        public FacebookClient(IFacebookApplication facebookApplication)
        {
            if (facebookApplication != null)
            {
                if (!string.IsNullOrEmpty(facebookApplication.AppId) && !string.IsNullOrEmpty(facebookApplication.AppSecret))
                {
                    this.AccessToken = string.Concat(facebookApplication.AppId, "|", facebookApplication.AppSecret);
                }
            }
        }
    }
}