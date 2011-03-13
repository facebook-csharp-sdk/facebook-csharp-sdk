using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;

namespace CSASPNETWebsiteRegistrationForm
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // YOU DONT NEED ANY OF THIS IN YOUR APPLICATION
            // THIS METHOD JUST CHECKS TO SEE IF YOU HAVE SETUP
            // THE SAMPLE. IF THE SAMPLE SEE THE GETTING STARTED
            // INFORMATION.

            if (FacebookApplication.Current.AppId == "{appid}")
            {
                pnlGettingStarted.Visible = true;
                pnlRegistration.Visible = false;
            }
            else
            {
                pnlGettingStarted.Visible = false;
                pnlRegistration.Visible = true;
            }
        }

        public string RegistrationUrl
        {
            get
            {
                return string.Format(
                        "http://www.facebook.com/plugins/registration.php?client_id={0}&redirect_uri={1}&fields={2}",
                        FacebookApplication.Current.AppId,
                        HttpUtility.UrlEncode("http://localhost:5000/fbregcallback.aspx"),
                        HttpUtility.UrlEncode("[{\"name\":\"name\"},{\"name\":\"email\"},{\"name\":\"location\"},{\"name\":\"gender\"},{\"name\":\"birthday\"},{\"name\":\"password\",\"view\":\"not_prefilled\"},{\"name\":\"like\",\"description\":\"Doyoulikethisplugin?\",\"type\":\"checkbox\",\"default\":\"checked\"},{\"name\":\"phone\",\"description\":\"PhoneNumber\",\"type\":\"text\"},{\"name\":\"captcha\"}]"));
            }
        }
    }
}