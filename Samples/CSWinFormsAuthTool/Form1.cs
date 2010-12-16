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

namespace Facebook.Samples.AuthenticationTool
{
    public partial class Form1 : Form
    {
        protected const string kApiSecret = "{Your secret goes here}";
        protected const string kAppId = "{your App Id Goes Here}";
        protected const bool kCookieSupport = false;

        public Form1()
        {
            InitializeComponent();
            permissions.Items.AddRange(ExtendedPermissions.Permissions);
        }

        private IFacebookSettings GetSettings()
        {
            IFacebookSettings facebookSettings;
            if (string.IsNullOrEmpty(appId.Text))
            {
                facebookSettings = new FacebookSettings { ApiSecret = kApiSecret, AppId = kAppId, CookieSupport = kCookieSupport };
            }
            else
            {
                facebookSettings = new FacebookSettings
                {
                    AppId = appId.Text.Trim(),
                };
            }
            return facebookSettings;
        }

        private void login_Click(object sender, EventArgs e)
        {
#if NET35

#else
            FacebookApp app = new FacebookApp(GetSettings());

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

            Uri loginUrl = app.GetLoginUrl(parameters);

            webBrowser1.Navigate(loginUrl);
            webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);
#endif
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.Fragment.Contains("access_token"))
            {
                NameValueCollection queryString = new NameValueCollection();
                string[] parts = e.Url.Fragment.Substring(1, e.Url.Fragment.Length - 1).Split('&');
                foreach (var part in parts)
                {
                    var nameValue = part.Split('=');
                    queryString.Add(nameValue[0], nameValue[1]);
                }
                accessToken.Text = Uri.UnescapeDataString(queryString["access_token"]);
            }
        }

        private void loadProfiles_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(userId.Text))
            {
                FacebookApp app = new FacebookApp(GetSettings());
                dynamic result = app.Query(string.Format("SELECT id,name,type FROM profile WHERE id={0}", userId.Text));
                profiles.Items.AddRange(result);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appId.Text = GetSettings().AppId;
        }
    }
}