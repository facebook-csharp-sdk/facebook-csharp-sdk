using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Facebook.OAuth;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            permissions.Items.AddRange(ExtendedPermissions.Permissions);
        }

        private void login_Click(object sender, EventArgs e)
        {
            bool isReady = true;
            if (String.IsNullOrEmpty(appId.Text.Trim()))
            {
                isReady = false;
                appId.Text = "REQUIRED!";
            }
            if (String.IsNullOrEmpty(appSecret.Text.Trim()))
            {
                isReady = false;
                appSecret.Text = "REQUIRED!";
            }
            if (!isReady)
            {
                return;
            }

            var facebookSettings = new FacebookSettings
            {
                AppId = appId.Text.Trim(),
                AppSecret = appSecret.Text.Trim(),
            };

            FacebookApp app = new FacebookApp(facebookSettings);

            dynamic parameters = new ExpandoObject();
            parameters.type = "user_agent";
            //parameters.redirect_uri = "http://www.facebook.com/connect/login_success.html";

            StringBuilder perms = new StringBuilder();
            foreach (var permission in permissions.CheckedItems)
            {
                perms.Append(permission);
                perms.Append(",");
            }
            parameters.scope = perms.ToString();

            var oAuthClientAuthorizer = new FacebookOAuthClientAuthorizer(appId.Text.Trim(), appSecret.Text.Trim(), null);
            Uri loginUrl = oAuthClientAuthorizer.GetDesktopLoginUri(null);

            webBrowser1.Navigate(loginUrl);
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);
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