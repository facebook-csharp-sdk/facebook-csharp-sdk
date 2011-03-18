using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Facebook.Samples.AuthenticationTool
{
    public partial class Info : Form
    {
        private readonly string _accessToken;

        public Info(string accessToken)
        {
            _accessToken = accessToken;

            InitializeComponent();

            GraphApiAsyncExample();
            LegacyRestApiAsyncExample();
            FqlAsyncSample();
            FqlMultiQueryAsyncSample();
            GraphApiBatchRequestAsyncSample();

            GraphApiExample();
        }

        private void GraphApiExample()
        {
            // note: avoid using synchronous methods if possible as it will block the thread until the result is received
            // use async methods instead. see: GraphApiAsyncExample()
            var fb = new FacebookClient(_accessToken);

            try
            {
                dynamic result = fb.Get("/me");
                var name = result.name;

                lnkName.Text = "Hi " + name;
                lnkName.LinkClicked += (o, e) => Process.Start(result.link);

                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                picProfilePic.LoadAsync(string.Format("https://graph.facebook.com/{0}/picture?type={1}", result.id, "square"));
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GraphApiAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted +=
                (o, e) =>
                {
                    // note: remember to always check the error for async methods
                    if (e.Error == null)
                    {
                        // there was no error, so process the result
                        dynamic result = e.GetResultData();
                        // note: for performance remeber to cache the GetResultData() or GetResultData<T>()
                        // every time you call it will deserialize the json string into object.

                        lblFirstName.Text = "First name: " + result.first_name;
                        lblLastName.Text = "Last name: " + result.last_name;
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            dynamic parameters = new ExpandoObject();
            parameters.fields = "first_name,last_name";

            //// if dynamic keyword is not supported.
            //var parameters = new Dictionary<string, object>
            //                     {
            //                         { "fields", "first_name,last_name" }
            //                     };

            fb.GetAsync("/me", parameters);
        }

        private void LegacyRestApiAsyncExample()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        // there was no error, so process the result
                        bool result = e.GetResultData<bool>();

                        // to get the non genric verion call without <T>.
                        // dynamic result = e.GetResultData();

                        chkIsFanOfFacebookSdk.Checked = result;

                        if (!result)
                        {
                            lnkFacebokSdkFan.Visible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            fb.GetAsync(new Dictionary<string, object>
                            {
                                { "method", "pages.isFan" },
                                { "page_id", "162171137156411" } // id of http://www.facebook.com/csharpsdk official page
                            });
        }

        private void FqlAsyncSample()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        // there was no error, so process the result

                        // note: avoid using JsonArray and JsonObject,
                        // instead cast
                        //  JsonArray  to (IList<object>) and
                        //  JsonObject to (IDictionary<string,object>)
                        var result = (IList<object>)e.GetResultData();

                        var count = result.Count;

                        lblTotalFriends.Text = string.Format("You have {0} friend(s).", count);
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            // query to get all the friends
            var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");
            fb.QueryAsync(query);
        }

        private void FqlMultiQueryAsyncSample()
        {
            var fb = new FacebookClient(_accessToken);

            fb.GetCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        dynamic result = e.GetResultData();

                        var resultForQuery1 = result[0].fql_result_set;
                        var resultForQuery2 = result[1].fql_result_set;

                        lblUserId.Text = "User id: " + resultForQuery1[0].uid;
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            var query1 = "SELECT uid FROM user WHERE uid=me()";
            var query2 = "SELECT profile_url FROM user WHERE uid=me()";

            fb.QueryAsync(new[] { query1, query2 });
        }

        private void btnPostToWall_Click(object sender, EventArgs eventArgs)
        {
            var fb = new FacebookClient(_accessToken);

            fb.PostCompleted +=
                (o, e) =>
                {
                    if (e.Error == null)
                    {
                        MessageBox.Show("Message posted successfully");
                        txtMessage.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show(e.Error.Message);
                    }
                };

            fb.PostAsync("/me/feed", new Dictionary<string, object> { { "message", txtMessage.Text } });
        }

        private void btnPostPicture_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                          {
                              Filter = "JPEG Files|*.jpg",
                              Title = "Select picture to upload"
                          };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var picture = File.ReadAllBytes(ofd.FileName);

                var fb = new FacebookClient(_accessToken);

                fb.PostCompleted +=
                    (o, args) =>
                    {
                        if (args.Error == null)
                        {
                            MessageBox.Show("Picture posted to wall successfully.");
                        }
                        else
                        {
                            MessageBox.Show(args.Error.Message);
                        }
                    };

                dynamic parameters = new ExpandoObject();
                parameters.caption = txtMessage.Text;
                parameters.method = "facebook.photos.upload";

                var mediaObject = new FacebookMediaObject
                                      {
                                          FileName = Path.GetFileName(ofd.FileName),
                                          ContentType = "image/jpeg"
                                      };
                mediaObject.SetValue(picture);
                parameters.source = mediaObject;

                fb.PostAsync(parameters);
            }
        }

        private void GraphApiBatchRequestAsyncSample()
        {
            var fb = new FacebookClient(_accessToken);

            var parameters =
                new
                    {
                        batch = new[]
                                    {
                                        new { method = "GET", relative_url = "me" },
                                        new { method = "GET", relative_url = "me/friends?limit=50" },
                                    }
                    };

            dynamic result = fb.Post(parameters);

            var result1 = result[0].body;
            var result2 = result[1].body;
        }

        private void lnkFacebokSdkFan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.facebook.com/csharpsdk");
        }
    }
}
