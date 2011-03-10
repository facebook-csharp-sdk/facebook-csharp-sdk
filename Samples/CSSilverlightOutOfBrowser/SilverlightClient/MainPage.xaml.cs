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
        private const string appId = "{app id}";

        private string[] requestedFbPermissions = new[] { "user_about_me" };

        // Note:
        // Host SilverlightClient.Web in IIS and not cassini (visual studio web server).
        // and change this url accordingly.
        // this silverlight app should be running out of browser in full trust mode.
        // due to security reasons, window.external.notify will not run in slfblogin.html if it
        // is from different domain.
        // so make sure you run this sample from the same domain where slfblogin.html file is located
        // i.e. http://localhost/fbsloob/
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

            var loginParameters = new Dictionary<string, object>
                                {
                                    { "response_type", "token" }
                                };

            var loginUrl = FacebookOAuthClient.GetLoginUrl(appId, new Uri(slfbloginUrl), requestedFbPermissions, loginParameters);

            FacebookLoginBrowser.Navigate(loginUrl);
        }

        private void FacebookLoginBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            FacebookOAuthResult authResult;
            if (FacebookOAuthResult.TryParse(e.Value, out authResult))
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

        private void loginSucceeded(FacebookOAuthResult authResult)
        {
            TitleBox.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            var fb = new FacebookClient(authResult.AccessToken);

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
                        MessageBox.Show(e.Error.Message);
                    }
                };

            fb.GetAsync("/me");
        }
    }
}