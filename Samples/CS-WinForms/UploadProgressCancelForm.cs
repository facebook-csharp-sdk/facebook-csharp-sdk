using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Facebook;

namespace CS_WinForms
{
    public partial class UploadProgressCancelForm : Form
    {
        private readonly string _accessToken;
        private string _filename;
        private FacebookClient _fb;

        public UploadProgressCancelForm(string accessToken)
        {
            _accessToken = accessToken;
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "jpeg (*.jpg) | *.jpg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _filename = ofd.FileName;
            }
            else
            {
                _filename = null;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_filename))
            {
                MessageBox.Show("Please select the image file first.");
                return;
            }

            var mediaObject = new FacebookMediaObject
            {
                ContentType = "image/jpeg",
                FileName = Path.GetFileName(_filename)
            }
                                  .SetValue(File.ReadAllBytes(_filename));

            progressBar1.Value = 0;

            var fb = new FacebookClient(_accessToken);
            fb.UploadProgressChanged += fb_UploadProgressChanged;
            fb.PostCompleted += fb_PostCompleted;

            // for cancellation
            _fb = fb;

            fb.PostAsync("/me/photos", new Dictionary<string, object> { { "source", mediaObject } });
        }

        public void fb_UploadProgressChanged(object sender, FacebookUploadProgressChangedEventArgs e)
        {
            progressBar1.BeginInvoke(new MethodInvoker(() =>
            {
                progressBar1.Value = e.ProgressPercentage;
            }));

        }

        public void fb_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Upload cancelled");
            }
            else if (e.Error == null)
            {
                // upload successful.
                MessageBox.Show(e.GetResultData().ToString());
            }
            else
            {
                // upload failed
                MessageBox.Show(e.Error.Message);
            }

            progressBar1.BeginInvoke(new MethodInvoker(() =>
            {
                progressBar1.Value = 0;
            }));

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_fb != null)
            {
                _fb.CancelAsync();
            }
        }
    }
}
