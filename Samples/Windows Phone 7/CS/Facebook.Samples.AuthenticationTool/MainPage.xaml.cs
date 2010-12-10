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
        private const string apiKey = "Your Application ID goes here";
        private string requestedFbPermissions = "user_about_me,"; //email,user_likes,user_checkins"; //"email,user_likes,user_checkins,publish_checkins"; //etc

        private string accessToken;

        private const string successUrl = "http://www.facebook.com/connect/login_success.html";

        private const string failedUrl = "http://www.facebook.com/connect/login_failure.html";

        private bool loggedIn = false;

        private FacebookApp fbApp;
        Uri loggingInUri;

        private void loginFailed(bool error)
        {
            // TODO: you should notify the user or do something else
        }

        // At this point we have an access token so we can get information from facebook
        private void loginSucceeded()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoPanel.Visibility = Visibility.Visible;

            fbApp.GetAsync("me", (val) =>
            {
                // Could also cast to our Dynamic object (but we are keeping things simple and familiar)
                var result = (IDictionary<string, object>)val.Result;
                Dispatcher.BeginInvoke(() => MyData.ItemsSource = result); // the lambda here sets the itemSource of the list box control which uses the ItemTemplate to render the items
            });
        }

        // We can use this event to capture the HTML or to make a script call (we use it right now to push an HTML fix)
        private void FacebookLoginBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Uncomment to see the generated html.. helps with determining the allowButtonPosition value
            //try
            //{
            //    var html = FacebookLoginBrowser.InvokeScript("eval", "document.documentElement.innerHTML");
            //    Debug.WriteLine("--------------------");
            //    Debug.WriteLine("content:" + html);
            //    Debug.WriteLine("--------------------");
            //}
            //catch (Exception ex) { }
            try
            {
                // HACK: Giant flaming hack to resolve an issue with Facebook's HTML in the allow screen
                // Eventually this will be unneeded, but for now it should resolve the issue and won't hurt things in the future
                FacebookLoginBrowser.InvokeScript("eval", "(function() { var aboveFooter=document.getElementById('platform_dialog_bottom_bar').previousSibling; document.getElementById('platform_dialog_bottom_bar').style.top=aboveFooter.offsetHeight + aboveFooter.offsetTop + 'px' })()");
            }
            catch (Exception ex) { }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            fbApp = new FacebookApp();
            FacebookLoginBrowser.Loaded += new RoutedEventHandler(FacebookLoginBrowser_Loaded);
        }

        // Browser control is loaded and fully ready for use
        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
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

            var parms = new Dictionary<String, object>();
            parms["display"] = "touch";
            parms["client_id"] = apiKey;
            parms["redirect_uri"] = successUrl;
            parms["cancel_url"] = failedUrl;
            parms["scope"] = requestedFbPermissions;
            parms["type"] = "user_agent";

            loggingInUri = fbApp.GetLoginUrl(parms);
            FacebookLoginBrowser.Navigate(loggingInUri);
        }

        private void FacebookLoginBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
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