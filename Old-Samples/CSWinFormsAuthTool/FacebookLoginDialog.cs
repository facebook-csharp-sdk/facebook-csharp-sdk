using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class FacebookLoginDialog : Form
    {
        private Uri _navigateUri;

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }

        public FacebookLoginDialog(string appId, string[] extendedPermissions, bool logout)
        {
            IDictionary<string, object> loginParameters = new Dictionary<string, object>
                                                              {
                                                                  { "response_type", "token" },
                                                                  { "display", "popup" }
                                                              };

            _navigateUri = FacebookOAuthClient.GetLoginUrl(appId, null, extendedPermissions, logout, loginParameters);

            InitializeComponent();
        }

        public FacebookLoginDialog(string appId, string[] extendedPermissions)
            : this(appId, extendedPermissions, false)
        {
        }

        private void FacebookLoginDialog_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate(_navigateUri.AbsoluteUri);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Url, out oauthResult))
            {
                this.FacebookOAuthResult = oauthResult;
                this.DialogResult = oauthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
            }
            else
            {
                this.FacebookOAuthResult = null;
            }
        }
    }
}
