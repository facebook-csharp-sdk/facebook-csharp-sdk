using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Browser;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Facebook;

namespace CS_Canvas_SL4
{
    public partial class MainPage : UserControl
    {
        private string _accessToken;

        public MainPage()
        {
            InitializeComponent();

            // note: make sure to register https:// prefix as ClientHttp as 
            // Facebook might return different error codes besides 200 or 404,
            // so in order for Facebook C# SDK to correctly parse errors as
            // FacebookApiException register this prefix before you make any
            // requests to Facebook server.
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(App.AccessToken))
            {
                _accessToken = App.AccessToken;
                GraphApiExample();
                FQLExample();
                FQLMultiQueryExample();
            }
        }

        private void GraphApiExample()
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
