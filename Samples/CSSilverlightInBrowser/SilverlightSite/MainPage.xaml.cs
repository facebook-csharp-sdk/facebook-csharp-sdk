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
        // make sure to set the appropriate app id, app secret and redirect uri in the
        // SilverlightSite.Web project in slfbinbrowserlogin.aspx file.

        private string appId = "{app id}";
        private string[] requestedFbPermissions = new[] { "user_about_me" };

        // Host SilverlightSite.Web in IIS and not cassini (visual studio web server).
        // and change this url accordingly.
        private const string slfbloginUrl = @"http://localhost/fbslinbrowser/slfbinbrowserlogin.aspx";

        private FacebookClient fb;

        private void loginSucceeded()
        {
            FbLoginButton.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            fb.GetCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        var result = (IDictionary<string, object>)e.GetResultData();
                        Dispatcher.BeginInvoke(() => InfoBox.ItemsSource = result);
                    }
                    else
                    {
                        // TODO: Need to let the user know there was an error
                    }
                };

            fb.GetAsync("/me");
        }

        private void FbLoginButton_Click(object sender, RoutedEventArgs e)
        {
            FbLoginButton.IsEnabled = false;
            LoginToFbViaJs();
        }

        #region JS Callable (& related) Code

        [ScriptableMember]
        public void LoginComplete(string accesstoken, string errorDescription)
        {
            if (string.IsNullOrEmpty(errorDescription) && !string.IsNullOrEmpty(accesstoken))
            {
                // we have access token.
                fb = new FacebookClient(accesstoken);
                loginSucceeded();
            }
            else
            {
                HtmlPage.Window.Alert(errorDescription);
            }
        }

        #endregion JS Callable (& related) Code

        #region Methods that call the Fb-Js API

        private void LoginToFbViaJs()
        {
            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "display", "popup" },
                                          { "response_type", "code" } // make it code and not access token for security reasons.
                                      };


            var loginUrl = FacebookOAuthClient.GetLoginUrl(appId, new Uri(slfbloginUrl), requestedFbPermissions, loginParameters);

            // don't make the response_type = token
            // coz it will be saved in the browser's history.
            // so others might hack it.
            // rather call ExchangeCodeForAccessToken to get access token in server side.
            // we need to this in server side and not in this silverlight app
            // so that the app secret doesn't get exposed to the client in case someone
            // reverse engineers this silverlight app.
            HtmlPage.Window.Eval(string.Format("fbLogin('{0}')", loginUrl));
        }

        #endregion Methods that call the Fb-Js API

        public MainPage()
        {
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("slObject", this);
        }
    }
}