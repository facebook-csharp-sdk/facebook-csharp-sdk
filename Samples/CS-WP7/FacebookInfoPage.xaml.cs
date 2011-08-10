using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using Facebook;
using Microsoft.Phone.Controls;

namespace CS_WP7
{
    public partial class FacebookInfoPage : PhoneApplicationPage
    {
        private string _accessToken;
        private IDictionary<string, object> _me;

        public FacebookInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _accessToken = NavigationContext.QueryString["access_token"];
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted += (o, args) =>
                                   {
                                       if (args.Error == null)
                                       {
                                           _me = (IDictionary<string, object>)args.GetResultData();

                                           Dispatcher.BeginInvoke(
                                               () =>
                                               {
                                                   LoadProfilePicture();
                                                   ProfileName.Text = "Hi " + _me["name"];
                                                   FirstName.Text = "First Name: " + _me["first_name"];
                                                   LastName.Text = "Last Name: " + _me["last_name"];
                                               });
                                       }
                                       else
                                       {
                                           Dispatcher.BeginInvoke(() => MessageBox.Show(args.Error.Message));
                                       }
                                   };

            // do a GetAsync me in order to get basic details of the user.
            fb.GetAsync("me");

            FqlSample();
            FqlMultiQuerySample();
        }

        private void LoadProfilePicture()
        {
            picProfile.Source = new BitmapImage(new Uri(string.Format("https://graph.facebook.com/{0}/picture", _me["id"])));
        }

        private void FqlSample()
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

                                           // now we can either cast it to IDictionary<string, object> or IList<object>
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

        private void FqlMultiQuerySample()
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

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    var result = (IDictionary<string, object>)args.GetResultData();
                    _lastMessageId = (string)result["id"];

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