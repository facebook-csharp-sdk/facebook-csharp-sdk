using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Facebook;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class MainPage : UserControl
    {
        private const string appId = "{your app id}";

        private string requestedFbPermissions = "user_about_me";

        // Host SilverlightClient.Web in IIS and not cassini (visual studio web server).
        // and change this url accordingly.
        // this silverlight app should be running out of browser in full trust mode.
        private const string slfbloginUrl = @"http://localhost/fbsloob/slfblogin.htm";

        private bool loggedIn = false;


        public MainPage()
        {
            InitializeComponent();
        }

        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                LoginToFacebook();
            }
        }

        private void LoginToFacebook()
        {
            TitleBox.Visibility = Visibility.Collapsed;
            FacebookLoginBrowser.Visibility = Visibility.Visible;
            InfoBox.Visibility = Visibility.Collapsed;

            var oauth = new FacebookOAuthClientAuthorizer
                            {
                                ClientId = appId,
                                RedirectUri = new Uri(slfbloginUrl)
                            };

            var paramaters = new Dictionary<string, object>
                                {
                                    { "display", "popup" },
                                    { "response_type", "token" },
                                    { "scope", this.requestedFbPermissions }
                                };

            var loginUrl = oauth.GetLoginUrl(paramaters);
            FacebookLoginBrowser.Navigate(loginUrl);
        }

        private void FacebookLoginBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            FacebookAuthenticationResult authResult;
            if (FacebookAuthenticationResult.TryParse(e.Value, out authResult))
            {
                if (authResult.IsSuccess)
                {
                    loggedIn = true;
                    loginSucceeded(authResult);
                }
                else
                {
                    MessageBox.Show(authResult.ErrorDescription);
                }
            }
        }

        private void loginSucceeded(FacebookAuthenticationResult authResult)
        {
            TitleBox.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            var fb = new FacebookApp(authResult.AccessToken);

            fb.GetAsync("me", val =>
            {
                var result = (IDictionary<string, object>)val.Result;
                Dispatcher.BeginInvoke(() => InfoBox.ItemsSource = result);
            });
        }
    }
}