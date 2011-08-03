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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AppId = ""
        private const string AppSecret = ""

        private readonly string[] _extendedPermissions = new[] { "user_about_me" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            var facebookLoginDialog = new FacebookLoginDialog(AppId, _extendedPermissions, true);
            facebookLoginDialog.ShowDialog();

            DisplayAppropriateMessage(facebookLoginDialog.FacebookOAuthResult);
        }

        private void DisplayAppropriateMessage(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult == null)
            {
                // most likely user closed the FacebookLoginDialog, so do nothing
                return;
            }

            if (facebookOAuthResult.IsSuccess)
            {
                // we only have code, but no access token, so in order to get access token use ExchangeCodeForAccessToken
                var oauth = new FacebookOAuthClient { AppId = AppId, AppSecret = AppSecret };

                dynamic accessTokenResult = oauth.ExchangeCodeForAccessToken(facebookOAuthResult.Code);
                string accessToken = accessTokenResult.access_token;

                var fb = new FacebookClient(accessToken);

                dynamic result = fb.Get("/me");
                MessageBox.Show("Hi " + result.name);
            }
            else
            {
                MessageBox.Show(facebookOAuthResult.ErrorDescription);
            }
        }
    }
}
