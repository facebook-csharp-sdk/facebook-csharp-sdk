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
        private const string appId = "{app id}";

        private bool loggedIn = false;

        private FacebookApp fbApp;

        // At this point we have an access token so we can get information from facebook
        private void loginSucceeded()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;
            InfoPanel.Visibility = Visibility.Visible;

            fbApp.GetAsync("me", (val) =>
            {
                // Could also cast to our Dynamic object (but we are keeping things simple and familiar)
                var result = (JsonObject)val.Result;
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

            var oauth = new FacebookOAuthClientAuthorizer
            {
                ClientId = appId,
                // RedirectUri = new Uri("http://www.facebook.com/connect/login_success.html") // by default the redirect_uri is http://www.facebook.com/connect/login_success.html
            };

            var paramaters = new Dictionary<string, object>
                                {
                                    { "response_type", "token" }
                                };

            var extendedPermissions = this.GetExtendedPermissions();

            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // add scope only if there is at last one extended permission
                paramaters["scope"] = extendedPermissions;
            }

            var loginUri = oauth.GetLoginUrl(paramaters);
            FacebookLoginBrowser.Navigate(loginUri);
        }

        private void FacebookLoginBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookAuthenticationResult authResult;
            if (FacebookAuthenticationResult.TryParse(e.Uri, out authResult))
            {
                if (authResult.IsSuccess)
                {
                    fbApp = new FacebookApp(authResult.AccessToken);
                    loginSucceeded();
                }
                else
                {
                    MessageBox.Show(authResult.ErrorDescription);
                }
            }
        }

        private string GetExtendedPermissions()
        {
            return "user_about_me,publish_stream";
        }
    }
}