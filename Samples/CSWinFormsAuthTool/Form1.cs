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

            var oauth = new FacebookOAuthClientAuthorizer
                            {
                                ClientId = appId.Text.Trim(),
                                // RedirectUri = new Uri("http://www.facebook.com/connect/login_success.html") // by default the redirect_uri is http://www.facebook.com/connect/login_success.html
                            };

            var paramaters = new Dictionary<string, object>
                                {
                                    { "type", "user_agent" } // add type=user_agent so we don't need to exchange code for access_token
                                };

            var extendedPermissions = this.GetExtendedPermissions();

            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // add scope only if there is at last one extended permission
                paramaters["scope"] = extendedPermissions;
            }

            var loginUri = oauth.GetLoginUri(paramaters);
            webBrowser1.Navigate(loginUri);
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            FacebookAuthenticationResult authResult;
            if (FacebookAuthenticationResult.TryParse(e.Url, out authResult))
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