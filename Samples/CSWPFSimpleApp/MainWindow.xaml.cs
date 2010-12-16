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
        private const string apiKey = "{Your Api Key goes here}";

        private string requestedFbPermissions = "user_about_me";

        private const string successUrl = "http://www.facebook.com/connect/login_success.html";

        private const string failedUrl = "http://www.facebook.com/connect/login_failure.html";

        private bool loggedIn = false;

        Uri loggingInUri;

        private string accessToken;

        private FacebookApp fbApp;

        private void loginFailed(bool error)
        {
            // TODO: you should notify the user or do something else
        }

        private void loginSucceeded()
        {
            TitleBox.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            fbApp.GetAsync("me", (val) =>
            {
                var result = (IDictionary<string, object>)val.Result;

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                    new Action(delegate() { InfoBox.ItemsSource = result; }));
            });
        }

        public MainWindow()
        {
            InitializeComponent();
            fbApp = new FacebookApp();
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

            dynamic parms = new System.Dynamic.ExpandoObject();
            //parms.display = "popup";
            parms.client_id = apiKey;
            parms.redirect_uri = successUrl;
            parms.cancel_url = failedUrl;
            parms.scope = requestedFbPermissions;
            parms.type = "user_agent";

            // TODO: figure out why this temporary hack is necessary
            loggingInUri = new Uri(fbApp.GetLoginUrl(parms).ToString());

            FacebookLoginBrowser.Source = (loggingInUri);
        }

        void FacebookLoginBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine(e.Uri);
            if (e.Uri == loggingInUri)
            {
                // this event will fire when we first navigate the browser control to log in
                // when the Uri is the same as the one for logging in then we can skip this one
                // (we want the one that happens after logging in)
                return;
            }
            var uri = e.Uri;

            if (successUrl.EndsWith(uri.LocalPath))
            {
                // We're on the success page
                accessToken = "";
                string[] queryVars;
                if (String.IsNullOrEmpty(uri.Fragment) && uri.Query != null)
                {
                    queryVars = uri.Query.Split('&');
                }
                else
                {
                    queryVars = uri.Fragment.Split('&');
                }
                foreach (var line in queryVars)
                {
                    var KeyValue = line.Split('=');
                    if (KeyValue.Length > 1 && KeyValue[0].Contains("access_token"))
                    {
                        accessToken = KeyValue[1];
                    }
                }

                if (String.IsNullOrEmpty(accessToken))
                {
                    // TODO: if this happens you might have an error in your app or your AppId (consult our docs on proper setup)
                    loginFailed(true);
                    return;
                }

                loggedIn = true;

                fbApp = new FacebookApp(accessToken);

                loginSucceeded();
            }
            if (failedUrl.EndsWith(uri.LocalPath))
            {
                // We're on the failed page
                loginFailed(false);
            }
        }
    }
}