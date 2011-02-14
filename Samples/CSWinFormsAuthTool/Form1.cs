namespace Facebook.Samples.AuthenticationTool
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            permissions.Items.AddRange(ExtendedPermissions.Permissions);
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (!this.ValidateRequiredFields())
            {
                return;
            }

            var oauth = new FacebookOAuthClient
                            {
                                ClientId = appId.Text.Trim(),
                                // RedirectUri = new Uri("http://www.facebook.com/connect/login_success.html") // by default the redirect_uri is http://www.facebook.com/connect/login_success.html
                            };

            var paramaters = new Dictionary<string, object>
                                {
                                    { "display", "popup" },
                                    { "response_type", "token" }
                                };

            var extendedPermissions = this.GetExtendedPermissions();

            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // add scope only if there is at last one extended permission
                paramaters["scope"] = extendedPermissions;
            }

            var loginUri = oauth.GetLoginUrl(paramaters);
            webBrowser1.Navigate(loginUri.AbsoluteUri);
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            FacebookOAuthResult authResult;
            if (FacebookOAuthResult.TryParse(e.Url, out authResult))
            {
                if (authResult.IsSuccess)
                {
                    accessToken.Text = authResult.AccessToken;
                }
                else
                {
                    MessageBox.Show(authResult.ErrorDescription);
                }
            }
        }

        private string GetExtendedPermissions()
        {
            var extendedPermissions = new StringBuilder();
            foreach (var permission in permissions.CheckedItems)
            {
                extendedPermissions.Append(permission);
                extendedPermissions.Append(",");
            }

            if (extendedPermissions.Length > 0)
            {
                // remove the last comma
                --extendedPermissions.Length;
            }

            return extendedPermissions.ToString();
        }

        private bool ValidateRequiredFields()
        {
            bool isReady = true;
            if (String.IsNullOrEmpty(appId.Text.Trim()))
            {
                isReady = false;
                appId.Text = "REQUIRED!";
            }

            return isReady;
        }
    }
}