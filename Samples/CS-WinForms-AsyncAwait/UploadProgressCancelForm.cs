using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;

namespace CS_WinForms
{
    public partial class UploadProgressCancelForm : Form
    {
        private readonly FacebookClient _fb;
        private string _filename;
        CancellationTokenSource _cts;

        public UploadProgressCancelForm(string accessToken)
            : this(new FacebookClient(accessToken))
        {
        }

        public UploadProgressCancelForm(FacebookClient fb)
        {
            if (fb == null)
                throw new ArgumentNullException("fb");

            // if you are using XTaskAsync methods, you can use the same FacebookClient to execute
            // multiple asynchronous requests.
            _fb = fb;

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

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_filename))
            {
                MessageBox.Show("Please select the image file first.");
                return;
            }

            _cts = new CancellationTokenSource();

            var mediaObject = new FacebookMediaObject
            {
                ContentType = "image/jpeg",
                FileName = Path.GetFileName(_filename)
            }.SetValue(File.ReadAllBytes(_filename));

            var uploadProgress = new Progress<FacebookUploadProgressChangedEventArgs>();
            uploadProgress.ProgressChanged += (o, args) => progressBar1.Value = args.ProgressPercentage;

            try
            {
                await _fb.PostTaskAsync("me/photos", new Dictionary<string, object> { { "source", mediaObject } }, null, _cts.Token, uploadProgress);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show("Upload Cancelled");
            }
            catch (FacebookApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                progressBar1.Value = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null)
                _cts.Cancel();
        }
    }
}
