using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Facebook;

namespace SL4_InBrowser
{
    [ScriptableType]
    public partial class MainPage : UserControl
    {
        private string apiKey = "{Your api key goes here}";
        private string RequestedFbPermissions = "user_about_me";

        private const string successUrl = @"http://localhost:18201/LoginSuccessful.htm";
        private const string failedUrl = @"http://localhost:18201/LoginUnsuccessful.htm";

        private FacebookApp fbApp;

        private bool loggedIn;
        private string accessToken;

        private void failedLogin()
        {
            FbLoginButton.Visibility = Visibility.Visible;
            InfoBox.Visibility = Visibility.Collapsed;
            FbLoginButton.IsEnabled = true;
            // TODO: Should do something to let the user know that they
            // aren't logged in and that they can't proceed
        }

        private void loginSucceeded()
        {
            FbLoginButton.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            fbApp.GetAsync("me", (val) =>
            {
                if (val.Error == null)
                {
                    var result = (IDictionary<string, object>)val.Result;
                    Dispatcher.BeginInvoke(() => InfoBox.ItemsSource = result);
                }
                else
                {
                    // TODO: Need to let the user know there was an error
                    //failedLogin();
                }
            });
        }

        private void FbLoginButton_Click(object sender, RoutedEventArgs e)
        {
            FbLoginButton.IsEnabled = false;
            LoginToFbViaJs();
        }

        #region JS Callable (& related) Code

        [ScriptableMember]
        public void LoggedIn(string uri) //string sessionKey, string sessionSecret, int expires, string userId, string allowedPermissions)
        {
            var qsLoc = uri.IndexOfAny(new[] { '?', '#' });

            var queryString = (qsLoc == -1 ? uri : uri.Substring(qsLoc));
            var accessToken = "";
            var queryVars = queryString.Split('&');
            foreach (var line in queryVars)
            {
                var KeyValue = line.Split('=');
                if (KeyValue.Length > 1 && KeyValue[0].Contains("access_token"))
                {
                    accessToken = KeyValue[1];
                }
            }
            // Logged in with a session (convert the session to a secret and we are in business)
            if (string.IsNullOrEmpty(accessToken))
            {
                failedLogin();
                return;
            }
            this.accessToken = accessToken;

            loggedIn = true;
            fbApp = new FacebookApp(accessToken);
            loginSucceeded();
        }

        [ScriptableMember]
        public void LoggedInFailed()
        {
            failedLogin();
        }

        #endregion JS Callable (& related) Code

        #region Methods that call the Fb-Js API

        private void LoginToFbViaJs()
        {
            //// Now we can call the JS Api to checkLogin
            dynamic parms = new System.Dynamic.ExpandoObject();
            parms.display = "popup";
            parms.client_id = apiKey;
            parms.redirect_uri = successUrl;
            parms.cancel_url = failedUrl;
            parms.scope = RequestedFbPermissions;
            parms.type = "user_agent";

            var uri = fbApp.GetLoginUrl(parms);

            HtmlPage.Window.Eval(String.Format("fbLogin('{0}')", uri));
        }

        #endregion Methods that call the Fb-Js API

        public MainPage()
        {
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("slObject", this);
            fbApp = new FacebookApp();
        }
    }
}