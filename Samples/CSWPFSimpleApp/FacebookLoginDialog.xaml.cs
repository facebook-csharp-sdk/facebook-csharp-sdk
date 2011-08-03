using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Facebook.Samples.AuthenticationTool
{
    /// <summary>
    /// Interaction logic for FacebookLoginDialog.xaml
    /// </summary>
    public partial class FacebookLoginDialog : Window
    {
        private readonly Uri _navigateUrl;

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }

        public FacebookLoginDialog(string appId, string[] extendedPermissions, bool logout)
        {
            // there is a bug in wpf browser which omits anything in the fragment part of the uri
            // due to this bug we can't see the access token (#access_token=....) in the uri
            // to fix this either use win forms for login only or ask for code instead of token.

            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "response_type", "code" } 
                                      };

            _navigateUrl = FacebookOAuthClient.GetLoginUrl(appId, null, extendedPermissions, loginParameters);
            InitializeComponent();
        }

        public FacebookLoginDialog(string appId, string[] extendedPermissions)
            : this(appId, extendedPermissions, false)
        {
        }

        private void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
            {
                FacebookOAuthResult = oauthResult;
                DialogResult = oauthResult.IsSuccess;
            }
            else
            {
                FacebookOAuthResult = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(_navigateUrl.AbsoluteUri);
        }
    }
}
