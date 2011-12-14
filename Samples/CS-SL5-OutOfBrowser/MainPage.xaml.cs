using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Browser;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Facebook;
using System.Threading.Tasks;

namespace CS_SL5_OutOfBrowser
{
    public partial class MainPage : UserControl
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

        // Note:
        // Host CS-SL5-OutOfBrowser.Web in IIS (or IIS express) and not Cassini (visual studio web server).
        // and change this url accordingly.
        // this Silverlight app should be running out of browser in full trust mode.
        // due to security reasons, window.external.notify will not run silverlight_facebook_callback.htm if it
        // is from different domain.
        // make sure you run this sample from the same domain where SilverlightFacebookCallback.aspx file is located
        // i.e. http://localhost:60259/
        // make sure to also set the SiteUrl to http://localhost:60259/
        private const string _silverlightFacebookCallback = "http://localhost:60259/SilverlightFacebookCallback.aspx";

        private bool _loggedIn;
        private string _accessToken;
        private FacebookClient _fb;
        private TaskScheduler _ui;

        public MainPage()
        {
            InitializeComponent();

            _ui = TaskScheduler.FromCurrentSynchronizationContext();

            // note: make sure to register https:// prefix as ClientHttp as 
            // Facebook might return different error codes besides 200 or 404,
            // so in order for Facebook C# SDK to correctly parse errors as
            // FacebookApiException register this prefix before you make any
            // requests to Facebook server.
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loggedIn)
            {
                LoginToFacebook();
            }
        }

        private void LoginToFacebook()
        {
            TitleBox.Visibility = Visibility.Collapsed;
            webBrowser1.Visibility = Visibility.Visible;
            InfoBox.Visibility = Visibility.Collapsed;

            dynamic loginParameters = new ExpandoObject();
            loginParameters.display = "popup";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            // note: there is a bug in wpf browser control which ignores the fragment part (#) of the url
            // so we cannot get the access token. To fix this, set response_type to code as code is set in
            // the querystring.
            loginParameters.response_type = "code";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(ExtendedPermissions))
            {
                // A comma-delimited list of permissions
                loginParameters.scope = ExtendedPermissions;
            }

            // Make sure to set the app id.
            var oauthClient = new FacebookOAuthClient { AppId = AppId, RedirectUri = new Uri(_silverlightFacebookCallback) };

            var loginUrl = oauthClient.GetLoginUrl(loginParameters);

            webBrowser1.Navigate(loginUrl);
        }

        private void webBrowser1_ScriptNotify(object sender, NotifyEventArgs e)
        {
            dynamic result = JsonSerializer.Current.DeserializeObject(e.Value);
            string errorDescription = result.error_description;
            string accessToken = result.access_token;

            if (string.IsNullOrEmpty(errorDescription))
            {
                _loggedIn = true;
                LoginSucceeded(accessToken);
            }
            else
            {
                MessageBox.Show(errorDescription);
            }
        }

        private void LoginSucceeded(string accessToken)
        {
            TitleBox.Visibility = Visibility.Visible;
            webBrowser1.Visibility = Visibility.Collapsed;
            InfoBox.Visibility = Visibility.Visible;

            _accessToken = accessToken;
            _fb = new FacebookClient(accessToken);

            GraphApiGetExample();
            FQLExample();
            FQLMultiQueryExample();
        }

        private void GraphApiGetExample()
        {
            _fb.GetTaskAsync("me").ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        MessageBox.Show(t.Exception.GetBaseException().Message);
                        return;
                    }

                    dynamic me = t.Result;
                    picProfile.Source = new BitmapImage(new Uri(string.Format("https://graph.facebook.com/{0}/picture", me.id)));

                    ProfileName.Text = "Hi " + me.name;
                    FirstName.Text = "First Name: " + me.first_name;
                    LastName.Text = "Last Name: " + me.last_name;

                }, _ui);
        }

        private void FQLExample()
        {
            // query to get all the friends
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

            _fb.QueryTaskAsync(query).ContinueWith(
                t =>
                {
                    if (t.IsCanceled)
                    {
                        // for this example, we can ignore as we don't allow this
                        // example to be cancelled.
                        return;
                    }
                    if (t.IsFaulted)
                    {
                        MessageBox.Show(t.Exception.GetBaseException().Message);
                        return;
                    }

                    dynamic result = t.Result;
                    var count = result.Count; // since result is IList<object>, we can call Count property of IList<object>

                    TotalFriends.Text = string.Format("You have {0} friend(s).", count);
                }, _ui);
        }

        private void FQLMultiQueryExample()
        {
            var query1 = "SELECT uid FROM user WHERE uid=me()";
            var query2 = "SELECT profile_url FROM user WHERE uid=me()";

            _fb.QueryTaskAsync(query1, query2).ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        MessageBox.Show(t.Exception.GetBaseException().Message);
                        return;
                    }

                    dynamic result = t.Result;
                    var resultForQuery1 = result[0].fql_result_set;
                    var resultForQuery2 = result[1].fql_result_set;

                }, _ui);
        }

        private string _lastMessageId;
        private void PostToWall_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Enter message.");
                return;
            }

            var parameters = new Dictionary<string, object>();
            parameters["message"] = txtMessage.Text;

            _fb.PostTaskAsync("me/feed", parameters).ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        MessageBox.Show(t.Exception.GetBaseException().Message);
                        return;
                    }

                    dynamic result = t.Result;
                    _lastMessageId = result.id;

                    MessageBox.Show("Message Posted successfully");

                    txtMessage.Text = string.Empty;
                    btnDeleteLastMessage.IsEnabled = true;
                }, _ui);
        }

        private void DeleteLastMessage_Click(object sender, RoutedEventArgs e)
        {
            btnDeleteLastMessage.IsEnabled = false;

            _fb.DeleteTaskAsync(_lastMessageId).ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        MessageBox.Show(t.Exception.GetBaseException().Message);
                        return;
                    }

                    MessageBox.Show("Message deleted successfully");
                    btnDeleteLastMessage.IsEnabled = false;
                }, _ui);
        }
    }
}
