namespace Facebook.Samples.AuthenticationTool
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using Facebook.OAuth;

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
            bool isReady = true;
            if (String.IsNullOrEmpty(appId.Text.Trim()))
            {
                isReady = false;
                appId.Text = "REQUIRED!";
            }

            if (!isReady)
            {
                return;
            }

            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "type", "user_agent" }
                                      };

            var perms = new StringBuilder();

            foreach (var permission in permissions.CheckedItems)
            {
                perms.Append(permission);
                perms.Append(",");
            }

            if (permissions.CheckedItems.Count > 0)
            {
                --perms.Length; // remove the last comma
                loginParameters["scope"] = perms.ToString();
            }

            var authorizer = new FacebookOAuthClientAuthorizer(appId.Text.Trim(), null, null);
            var loginUri = authorizer.GetDesktopLoginUri(loginParameters);

            webBrowser1.Navigate(loginUri);
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            FacebookAuthenticationResult authResult;
            if (FacebookAuthenticationResult.TryParse(e.Url, out authResult))
            {
                accessToken.Text = authResult.AccessToken;
            }
        }
    }
}