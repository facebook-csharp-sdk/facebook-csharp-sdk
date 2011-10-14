using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;

namespace CS_WinForms
{
    public partial class FacebookInfoDialog : Form
    {
        private readonly FacebookClient _fb;

        public FacebookInfoDialog(string accessToken)
            : this(new FacebookClient(accessToken))
        {
        }

        public FacebookInfoDialog(FacebookClient fb)
        {
            if (fb == null)
                throw new ArgumentNullException("fb");

            // if you are using XTaskAsync methods, you can use the same FacebookClient to execute
            // multiple asynchronous requests.
            _fb = fb;

            InitializeComponent();
        }

        private void FacebookInfoDialog_Load(object sender, EventArgs e)
        {
            GetUserProfilePicture();

            GraphApiAsyncDynamicExample();
            GraphApiAsyncParametersDictionaryExample();
            GraphApiAsyncParametersExpandoObjectExample();
            GraphApiAsyncParametersInPathExample();

            LegacyRestApiAsyncExample();

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

                picProfile.LoadAsync(profilePictureUrl);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
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
                lblUserId.Text = "User Id: " + id;
                lnkName.Text = "Hi " + name;
                lnkName.LinkClicked += (o, e) => System.Diagnostics.Process.Start(result.link);

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
                MessageBox.Show(ex.Message);
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

                lblFirstName.Text = "First Name: " + result.first_name;

            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
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

                lblLastName.Text = "Last Name: " + result.last_name;
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void GraphApiAsyncParametersInPathExample()
        {
            try
            {
                dynamic result = await _fb.GetTaskAsync("me?fields=first_name,last_name");

                dynamic firstName = result.first_name;
                dynamic lastName = result.last_name;

                //// this is especially useful for paged data (result.paging.next and result.paging.previous)
                //// and your path can also contain the full graph url (https://graph.facebook.com/"
                //var nextPath = "https://graph.facebook.com/me/likes?limit=3&access_token=xxxxxxxxxxx&offset=3";
                //dynamic nextResult = fb.Get(nextPath);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LegacyRestApiAsyncExample()
        {
            try
            {
                dynamic parameters = new ExpandoObject();
                // any parameter that has "method" automatically is treated as rest api.
                parameters.method = "pages.isFan";
                parameters.page_id = "162171137156411";  // id of http://www.facebook.com/csharpsdk official page

                // for rest api only, parameters is enough
                // the rest method is determined by parameters.method
                dynamic isFan = await _fb.GetTaskAsync(parameters);
                chkCSharpSdkFan.Checked = isFan;
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
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

                lblTotalFriends.Text = string.Format("You have {0} friend(s).", result.Count);
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                    MessageBox.Show(((Exception)result[0]).Message);
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
                MessageBox.Show(ex.Message);
            }
        }

        private string _lastMessageId;

        private async void btnPostToWall_Click(object sender, EventArgs e)
        {
            try
            {
                dynamic parameters = new ExpandoObject();
                parameters.message = txtMessage.Text;

                dynamic result = await _fb.PostTaskAsync("me/feed", parameters);
                _lastMessageId = result.id;

                MessageBox.Show("Message Posted successfully");

                txtMessage.Text = string.Empty;
                btnDeleteLastMessage.Enabled = true;
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDeleteLastMessage_Click(object sender, EventArgs e)
        {
            btnDeleteLastMessage.Enabled = false;

            try
            {
                await _fb.DeleteTaskAsync(_lastMessageId);
                MessageBox.Show("Message deleted successfully");
                btnDeleteLastMessage.Enabled = false;
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void bntPostPicture_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "JPEG Files|*.jpg",
                    Title = "Select picture to upload"
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                dynamic parameters = new ExpandoObject();
                parameters.message = txtMessage.Text;
                parameters.source = new FacebookMediaObject
                                        {
                                            ContentType = "image/jpeg",
                                            FileName = Path.GetFileName(ofd.FileName)
                                        }.SetValue(File.ReadAllBytes(ofd.FileName));

                dynamic result = await _fb.PostTaskAsync("me/photos", parameters);

                MessageBox.Show("Picture uploaded successfully");
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnPostVideo_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "MP4 Files|*.mp4",
                    Title = "Select video to upload"
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                dynamic parameters = new ExpandoObject();
                parameters.message = txtMessage.Text;
                parameters.source = new FacebookMediaObject
                {
                    ContentType = "video/mp4",
                    FileName = Path.GetFileName(ofd.FileName)
                }.SetValue(File.ReadAllBytes(ofd.FileName));

                dynamic result = await _fb.PostTaskAsync("me/videos", parameters);

                MessageBox.Show("Video uploaded successfully");
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProgressAndCancellation_Click(object sender, EventArgs e)
        {
            var dlg = new UploadProgressCancelForm(_fb);
            dlg.ShowDialog();
        }
    }
}
