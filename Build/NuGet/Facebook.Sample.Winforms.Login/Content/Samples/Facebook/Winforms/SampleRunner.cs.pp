using System.Collections.Generic;
using System.Windows.Forms;
using Facebook;

namespace $rootnamespace$.Samples.Facebook.Winforms
{
    public class SampleRunner
    {
        public static void RunSample(string appId, string[] extendedPermissions)
        {
            var fbLoginDialog = new FacebookLoginDialog(appId, extendedPermissions, true);
            fbLoginDialog.ShowDialog();

            DisplayAppropriateMessage(fbLoginDialog.FacebookOAuthResult);
        }

        private static void DisplayAppropriateMessage(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult != null)
            {
                if (facebookOAuthResult.IsSuccess)
                {
                    var fb = new FacebookClient(facebookOAuthResult.AccessToken);

                    var result = (IDictionary<string, object>)fb.Get("/me");
                    var name = (string)result["name"];

                    MessageBox.Show("Hi " + name);
                }
                else
                {
                    MessageBox.Show(facebookOAuthResult.ErrorDescription);
                }
            }
        }
    }
}