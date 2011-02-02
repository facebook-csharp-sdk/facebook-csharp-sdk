using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Facebook.Samples.AuthenticationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string appId = "{appid}";

        private string requestedFbPermissions = "user_about_me";

        private bool loggedIn = false;

        Uri loggingInUri;

        private FacebookClient fb;

        private void loginSucceeded()
        {
            TitleBox.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            fb.GetAsync("me", (val) =>
            {
                var result = (IDictionary<string, object>)val.Result;

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                    new Action(delegate() { InfoBox.ItemsSource = result; }));
            });
        }

        public MainWindow()
        {
            InitializeComponent();
            fb = new FacebookClient();
            FacebookLoginBrowser.Loaded += new RoutedEventHandler(FacebookLoginBrowser_Loaded);
            FacebookLoginBrowser.Navigated += new NavigatedEventHandler(FacebookLoginBrowser_Navigated);
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

            var oauth = new FacebookOAuthClient
            {
                ClientId = appId,
                // RedirectUri = new Uri("http://www.facebook.com/connect/login_success.html") // by default the redirect_uri is http://www.facebook.com/connect/login_success.html
            };

            var paramaters = new Dictionary<string, object>
                                {
                                    { "display", "popup" },
                                    { "response_type", "token" },
                                    { "scope", requestedFbPermissions }
                                };

            var loginUri = oauth.GetLoginUrl(paramaters);
            FacebookLoginBrowser.Navigate(loginUri);
        }

        void FacebookLoginBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            FacebookOAuthResult authResult;
            if (FacebookOAuthResult.TryParse(e.Uri, out authResult))
            {
                if (authResult.IsSuccess)
                {
                    fb = new FacebookClient(authResult.AccessToken);
                    loginSucceeded();
                }
                else
                {
                    MessageBox.Show(authResult.ErrorDescription);
                }
            }
        }
    }
}