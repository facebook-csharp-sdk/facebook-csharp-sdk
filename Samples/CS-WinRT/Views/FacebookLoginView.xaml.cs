using System;
using System.Diagnostics;
using System.Dynamic;
using Facebook;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Controls;

namespace CS_WinRT.Views
{
    public sealed partial class FacebookLoginView
    {
        private const string AppId = ""

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,publish_stream,offline_access";

        private readonly Uri _loginUrl;

        public FacebookLoginView()
        {
            // Make sure to set the app id.
            var oauthClient = new FacebookOAuthClient { AppId = AppId };

            dynamic loginParameters = new ExpandoObject();

            loginParameters.response_type = "token";
            loginParameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(ExtendedPermissions))
            {
                // A comma-delimited list of permissions
                loginParameters.scope = ExtendedPermissions;
            }

            // when the Form is loaded navigate to the login url.
            _loginUrl = oauthClient.GetLoginUrl(loginParameters);

            InitializeComponent();
        }

        private void WebView1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            WebView1.LoadCompleted += WebView1_LoadCompleted;
            WebView1.Navigate(_loginUrl);
        }

        private void WebView1_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
                return;

            if (oauthResult.IsSuccess)
            {
                LoginSucceeded(oauthResult.AccessToken);
            }
            else
            {
                // user cancelled.
            }
        }

        private void LoginSucceeded(string accessToken)
        {
            Frame.Navigate(typeof(FacebookInfoView).FullName, accessToken);
        }
    }
}
