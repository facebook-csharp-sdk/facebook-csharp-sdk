using System;
using System.Collections.Generic;
using System.Dynamic;
using Facebook;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace CS_WinRT.Views
{
    public sealed partial class FacebookInfoView
    {
        private FacebookClient _fb;

        public FacebookInfoView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var accessToken = (string)e.Parameter;
            _fb = new FacebookClient(accessToken);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetUserProfilePicture();

            GraphApiAsyncDynamicExample();
            GraphApiAsyncParametersDictionaryExample();
            GraphApiAsyncParametersExpandoObjectExample();

            FqlAsyncExample();
            FqlMultiQueryAsyncExample();

            BatchRequestExample();
        }

        private async void GetUserProfilePicture()
        {
            try
            {
                dynamic result = await _fb.GetTaskAsync("me");
                string id = result.id;

                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}", id, "square");

                picProfile.Source = new BitmapImage(new Uri(profilePictureUrl));
            }
            catch (FacebookApiException ex)
            {
                // handel error message
            }
        }

        private async void GraphApiAsyncDynamicExample()
        {
            try
            {
                // instead of casting to IDictionary<string,object> or IList<object>
                // you can also make use of the dynamic keyword.
                dynamic result = await _fb.GetTaskAsync("me");

                // You can either access it this way, using the .
                dynamic id = result.id;
                dynamic name = result.name;

                // if dynamic you don't need to cast explicitly.
                ProfileName.Text = "Hi " + name;

                // or using the indexer
                dynamic firstName = result["first_name"];
                dynamic lastName = result["last_name"];

                // checking if property exist
                var localeExists = result.ContainsKey("locale");

                // you can also cast it to IDictionary<string,object> and then check
                var dictionary = (IDictionary<string, object>)result;
                localeExists = dictionary.ContainsKey("locale");
            }
            catch (FacebookApiException ex)
            {
                // handle error
            }
        }

        private async void GraphApiAsyncParametersDictionaryExample()
        {
            try
            {
                // additional parameters can be passed and 
                // must be assignable from IDictionary<string, object>
                var parameters = new Dictionary<string, object>();
                parameters["fields"] = "first_name,last_name";

                dynamic result = await _fb.GetTaskAsync("me", parameters);

                FirstName.Text = "First Name: " + result.first_name;

            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void GraphApiAsyncParametersExpandoObjectExample()
        {
            try
            {
                // additional parameters can be passed and 
                // must be assignable from IDictionary<string, object>
                dynamic parameters = new ExpandoObject();
                parameters.fields = "last_name";

                dynamic result = await _fb.GetTaskAsync("me", parameters);

                LastName.Text = "Last Name: " + result.last_name;
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void FqlAsyncExample()
        {
            try
            {
                // query to get all the friends
                var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

                // call the Query or QueryAsync method to execute a single fql query.
                dynamic result = await _fb.QueryTaskAsync(query);

                TotalFriends.Text = string.Format("You have {0} friend(s).", result.Count);
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }

        }

        private async void FqlMultiQueryAsyncExample()
        {
            try
            {
                var query1 = "SELECT uid FROM user WHERE uid=me()";
                var query2 = "SELECT profile_url FROM user WHERE uid=me()";

                // call the Query/QueryAsync/QueryTaskAsync method to execute a single fql query.
                // if there is more than one query, Query/QueryAsync/QueryTaskAsync method will automatically
                // treat it as multi-query.
                dynamic result = await _fb.QueryTaskAsync(new[] { query1, query2 });

                dynamic resultForQuery1 = result[0].fql_result_set;
                dynamic resultForQuery2 = result[1].fql_result_set;

                var uid = resultForQuery1[0].uid;

            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void BatchRequestExample()
        {
            try
            {
                dynamic result = await _fb.BatchTaskAsync(
                    new FacebookBatchParameter { HttpMethod = HttpMethod.Get, Path = "/4" },
                    new FacebookBatchParameter(HttpMethod.Get, "/me/friend", new Dictionary<string, object> { { "limit", 10 } }), // this should throw error
                    new FacebookBatchParameter("/me/friends", new { limit = 1 }) { Data = new { name = "one-friend", omit_response_on_success = false } }, // use Data to add additional parameters that doesn't exist
                    new FacebookBatchParameter { Parameters = new { ids = "{result=one-friend:$.data.0.id}" } },
                    new FacebookBatchParameter("{result=one-friend:$.data.0.id}/feed", new { limit = 5 }),
                    new FacebookBatchParameter().Query("SELECT name FROM user WHERE uid="), // fql
                    new FacebookBatchParameter().Query("SELECT first_name FROM user WHERE uid=me()", "SELECT last_name FROM user WHERE uid=me()") // fql multi-query
                    //,new FacebookBatchParameter(HttpMethod.Post, "/me/feed", new { message = "test status update" })
                    );

                // always remember to check individual errors for the batch requests.
                if (result[0] is Exception)
                {
                    // handle error message // MessageBox.Show(((Exception)result[0]).Message);
                    return;
                }

                dynamic first = result[0];
                string name = first.name;

                // note: incase the omit_response_on_success = true, result[x] == null

                // for this example, just comment it out.
                //if (result[1] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[2] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[3] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[4] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[5] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[6] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
                //if (result[7] is Exception)
                //    MessageBox.Show(((Exception)result[1]).Message);
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private string _lastMessageId;
        private async void PostToWall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic parameters = new ExpandoObject();
                parameters.message = txtMessage.Text;

                dynamic result = await _fb.PostTaskAsync("me/feed", parameters);
                _lastMessageId = result.id;

                //MessageBox.Show("Message Posted successfully");

                txtMessage.Text = string.Empty;
                btnDeleteLastMessage.IsEnabled = true;
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void DeleteLastMessage_Click(object sender, RoutedEventArgs e)
        {
            btnDeleteLastMessage.IsEnabled = false;

            try
            {
                await _fb.DeleteTaskAsync(_lastMessageId);
                //MessageBox.Show("Message deleted successfully");
                btnDeleteLastMessage.IsEnabled = false;
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }
    }
}
