using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string AppId = ""

        private readonly string[] _extendedPermissions = new[] { "user_about_me" };

        private bool _loggedIn;

        private FacebookClient _fbClient;

        // At this point we have an access token so we can get information from facebook
        private void LoginSucceeded()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoPanel.Visibility = Visibility.Visible;

            _fbClient.GetCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        var result = (IDictionary<string, object>)e.GetResultData();
                        Dispatcher.BeginInvoke(() => MyData.ItemsSource = result);
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            _fbClient.GetAsync("/me");
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _fbClient = new FacebookClient();
            FacebookLoginBrowser.Loaded += new RoutedEventHandler(FacebookLoginBrowser_Loaded);
        }

        // Browser control is loaded and fully ready for use
        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loggedIn)
            {
                LoginToFacebook();
            }
        }

        // This handles the display a little and also creates the initial URL to navigate to in the browser control
        private void LoginToFacebook()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Visible;
            InfoPanel.Visibility = Visibility.Collapsed;

            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "response_type", "token" }
                                          // { "display", "touch" } // by default for wp7 builds only (in Facebook.dll), display is set to touch.
                                      };

            var navigateUrl = FacebookOAuthClient.GetLoginUrl(AppId, null, _extendedPermissions, loginParameters);

            FacebookLoginBrowser.Navigate(navigateUrl);
        }

        private void FacebookLoginBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    _fbClient = new FacebookClient(oauthResult.AccessToken);
                    _loggedIn = true;
                    LoginSucceeded();
                }
                else
                {
                    MessageBox.Show(oauthResult.ErrorDescription);
                }
            }
        }
    }
}