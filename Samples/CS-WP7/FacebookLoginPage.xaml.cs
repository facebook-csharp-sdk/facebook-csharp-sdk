using System;
using System.Collections.Generic;
using System.Windows;
using Facebook;
using Microsoft.Phone.Controls;

namespace CS_WP7
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = ""

        private const string AppSecret = ""

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,publish_stream,offline_access";

        private readonly Uri _loginUrl;

        public FacebookLoginPage()
        {
            // NOTE: make sure to enable scripting for the web browser control.
            // <phone:WebBrowser x:Name="webBrowser1" IsScriptEnabled="True" />

            // Make sure to set the app id.
            var oauthClient = new FacebookOAuthClient { AppId = AppId };

            var loginParameters = new Dictionary<string, object>();

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            // note: there is a bug in wpf browser control which ignores the fragment part (#) of the url
            // so we cannot get the access token. To fix this, set response_type to code as code is set in
            // the querystring.
            loginParameters["response_type"] = "code";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(ExtendedPermissions))
            {
                // A comma-delimited list of permissions
                loginParameters["scope"] = ExtendedPermissions;
            }

            // when the Form is loaded navigate to the login url.
            _loginUrl = oauthClient.GetLoginUrl(loginParameters);

            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser1.Navigate(_loginUrl);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url
            // the url may be the result of OAuth 2.0 authentication.
            FacebookOAuthResult oauthResult;

            if (FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication.
                if (oauthResult.IsSuccess)
                {
                    var oauthClient = new FacebookOAuthClient { AppId = AppId, AppSecret = AppSecret };

                    // we got the code here
                    var code = oauthResult.Code;
                    oauthClient.ExchangeCodeForAccessTokenCompleted +=
                        (o, args) =>
                        {
                            // make sure to check that no error has occurred.
                            if (args.Error != null)
                            {
                                // make sure to access ui stuffs on the correct thread.
                                Dispatcher.BeginInvoke(
                                    () =>
                                    {
                                        MessageBox.Show(args.Error.Message);
                                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                                    });
                            }
                            else
                            {
                                var result = (IDictionary<string, object>)args.GetResultData();
                                var accessToken = (string)result["access_token"];

                                // make sure to access ui stuffs on the correct thread.
                                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/FacebookInfoPage.xaml?access_token=" + accessToken, UriKind.Relative)));
                            }
                        };

                    oauthClient.ExchangeCodeForAccessTokenAsync(code);
                }
                else
                {
                    // the user clicked don't allow or some other error occurred.
                    MessageBox.Show(oauthResult.ErrorDescription);
                }
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
            }
        }

    }
}