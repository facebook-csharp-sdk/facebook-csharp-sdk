using System;
using System.Collections.Generic;
using System.Net.Browser;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Facebook;
using System.Net;

namespace CS_SL4_OutOfBrowser
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
        // Host CS-SL4-OutOfBrowser.Web in IIS (or IIS express) and not Cassini (visual studio web server).
        // and change this url accordingly.
        // this Silverlight app should be running out of browser in full trust mode.
        // due to security reasons, window.external.notify will not run silverlight_facebook_callback.htm if it
        // is from different domain.
        // so make sure you run this sample from the same domain where silverlight_facebook_callback.htm file is located
        // i.e. http://localhost:1383/
        // make sure to also set the SiteUrl to http://localhost:1383/
        private const string _silverlightFacebookCallback = "http://localhost:1383/SilverlightFacebookCallback.aspx";

        private bool _loggedIn;
        private string _accessToken;

        public MainPage()
        {
            InitializeComponent();

            // note: make sure to register https:// prefix as ClientHttp as 
            // Facebook might return different error codes besides 200 or 404,
            // so inorder for Facebook C# SDK to correctly parse errors as
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

            var loginParameters = new Dictionary<string, object>
                                {
                                    { "display", "popup" }
                                };

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

            // Make sure to set the app id.
            var oauthClient = new FacebookOAuthClient { AppId = AppId, RedirectUri = new Uri(_silverlightFacebookCallback) };

            var loginUrl = oauthClient.GetLoginUrl(loginParameters);

            webBrowser1.Navigate(loginUrl);
        }

        private void webBrowser1_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var result = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(e.Value);
            string errorDescription = (string)result["error_description"];
            string accessToken = (string)result["access_token"];

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

            GraphApiGetExample();
            FQLExample();
            FQLMultiQueryExample();
        }

        private void GraphApiGetExample()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted += (o, e) =>
                                   {
                                       if (e.Error == null)
                                       {
                                           // make sure to reference Microsoft.CSharp if you want to use dynamic
                                           dynamic me = e.GetResultData();

                                           Dispatcher.BeginInvoke(
                                               () =>
                                               {
                                                   picProfile.Source = new BitmapImage(new Uri(string.Format("https://graph.facebook.com/{0}/picture", me.id)));

                                                   ProfileName.Text = "Hi " + me.name;
                                                   FirstName.Text = "First Name: " + me.first_name;
                                                   LastName.Text = "Last Name: " + me.last_name;
                                               });
                                       }
                                       else
                                       {
                                           Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                                       }
                                   };
            fb.GetAsync("me");
        }

        private void FQLExample()
        {
            var fb = new FacebookClient(_accessToken);

            // since FQL is internally a GET request,
            // make sure to add the GET event handler.
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(e.Error.Message);
                    });
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object> or dynamic
                    var result = (IList<object>)e.GetResultData();

                    var count = result.Count;

                    // since this is an async callback, make sure to be on the right thread
                    // when working with the UI.
                    Dispatcher.BeginInvoke(() =>
                    {
                        TotalFriends.Text = string.Format("You have {0} friend(s).", count);
                    });
                }
            };

            // query to get all the friends
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

            // call the Query or QueryAsync method to execute a single fql query.
            fb.QueryAsync(query);
        }

        private void FQLMultiQueryExample()
        {
            var fb = new FacebookClient(_accessToken);

            // since FQL multi-query is internally a GET request,
            // make sure to add the GET event handler.
            fb.GetCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(e.Error.Message);
                    });
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    var result = (IList<object>)e.GetResultData();

                    var resultForQuery1 = ((IDictionary<string, object>)result[0])["fql_result_set"];
                    var resultForQuery2 = ((IDictionary<string, object>)result[1])["fql_result_set"];

                    Dispatcher.BeginInvoke(() =>
                    {
                        // make sure to be on the right thread when working with ui.
                    });
                }
            };

            var query1 = "SELECT uid FROM user WHERE uid=me()";
            var query2 = "SELECT profile_url FROM user WHERE uid=me()";

            // call the Query or QueryAsync method to execute a single fql query.
            // if there is more than one query Query/QueryAsync method will automatically
            // treat it as multi-query.
            fb.QueryAsync(new[] { query1, query2 });
        }

        private string _lastMessageId;
        private void PostToWall_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Enter message.");
                return;
            }

            var fb = new FacebookClient(_accessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, args) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (args.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = args.Error;
                }
                else if (args.Error != null)
                {
                    // error occurred
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(args.Error.Message);
                    });
                }
                else
                {
                    // the request was completed successfully

                    dynamic result = args.GetResultData();
                    _lastMessageId = result.id;

                    // make sure to be on the right thread when working with ui.
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Message Posted successfully");

                        txtMessage.Text = string.Empty;
                        btnDeleteLastMessage.IsEnabled = true;
                    });
                }
            };

            var parameters = new Dictionary<string, object>();
            parameters["message"] = txtMessage.Text;

            fb.PostAsync("me/feed", parameters);
        }

        private void DeleteLastMessage_Click(object sender, RoutedEventArgs e)
        {
            btnDeleteLastMessage.IsEnabled = false;

            var fb = new FacebookClient(_accessToken);

            // make sure to add event handler for DeleteCompleted.
            fb.DeleteCompleted += (o, args) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (args.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    var cancellationError = args.Error;
                }
                else if (args.Error != null)
                {
                    // error occurred
                    Dispatcher.BeginInvoke(
                        () =>
                        {
                            MessageBox.Show(args.Error.Message);
                        });
                }
                else
                {
                    // the request was completed successfully

                    // make sure to be on the right thread when working with ui.
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Message deleted successfully");
                        btnDeleteLastMessage.IsEnabled = false;
                    });
                }
            };

            fb.DeleteAsync(_lastMessageId);
        }
    }
}
