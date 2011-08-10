using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Facebook;

namespace CS_WinForms
{
    /// <summary>
    /// Helper login dialog for Facebook.
    /// </summary>
    /// <remarks>
    /// For more information about Facebook OAuth Dialog refer to 
    /// https://developers.facebook.com/docs/reference/dialogs/oauth/
    /// </remarks>
    public partial class FacebookLoginDialog : Form
    {
        private readonly Uri _loginUrl;

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }

        public FacebookLoginDialog(string appId, string extendedPermissions)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");

            // Make sure to set the app id.
            var oauthClient = new FacebookOAuthClient { AppId = appId };

            IDictionary<string, object> loginParameters = new Dictionary<string, object>();

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            loginParameters["response_type"] = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            loginParameters["display"] = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                loginParameters["scope"] = extendedPermissions;
            }

            // when the Form is loaded navigate to the login url.
            _loginUrl = oauthClient.GetLoginUrl(loginParameters);

            InitializeComponent();
        }

        private void FacebookLoginDialog_Load(object sender, EventArgs e)
        {
            // make sure to use the AbsoluteUri.
            webBrowser1.Navigate(_loginUrl.AbsoluteUri);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url
            // the url may be the result of OAuth 2.0 authentication.

            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication.
                this.FacebookOAuthResult = oauthResult;
                this.DialogResult = FacebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                this.FacebookOAuthResult = null;
            }
        }
    }
}
