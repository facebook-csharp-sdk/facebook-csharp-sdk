using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    public partial interface IFacebookClient
    {
		event EventHandler<FacebookApiEventArgs> GetCompleted;
		event EventHandler<FacebookApiEventArgs> PostCompleted;
		event EventHandler<FacebookApiEventArgs> DeleteCompleted;
		event EventHandler<FacebookUploadProgressChangedEventArgs> UploadProgressChanged;
		void CancelAsync();
		void GetAsync(string path);
		void GetAsync(object parameters);
		void GetAsync(string path, object parameters);
		void GetAsync(string path, object parameters, object userState);
		void PostAsync(object parameters);
		void PostAsync(string path, object parameters);
		void PostAsync(string path, object parameters, object userState);
		void DeleteAsync(string path);
		void DeleteAsync(string path, object parameters, object userState);
    }
}
