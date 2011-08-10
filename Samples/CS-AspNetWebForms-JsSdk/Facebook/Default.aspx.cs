using System;
using System.Configuration;
using System.Dynamic;
using System.Web.Security;
using Facebook.Web;
using Facebook;

namespace CS_AspNetWebForms_JsSdk.Facebook
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var extendedPermissions = ConfigurationManager.AppSettings["extendedPermissions"].Split(',');

            if (!FacebookWebContext.Current.IsAuthorized(extendedPermissions))
            {
                // this sample actually does not make use of FormsAuthentication,
                // besides redirecting to the proper login url defined in web.config
                // this will also automatically add ReturnUrl.
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                // checking IsPostBack may reduce the number of requests to Facebook server.
                if (!IsPostBack)
                {
                    var fb = new FacebookWebClient();
                    dynamic me = fb.Get("me");

                    imgProfilePic.ImageUrl = string.Format("https://graph.facebook.com/{0}/picture", me.id);

                    lblName.Text = me.name;
                    lblFirstName.Text = me.first_name;
                    lblLastName.Text = me.last_name;
                }
            }
        }

        protected void btnPostToWall_Click(object sender, EventArgs e)
        {
            var fb = new FacebookWebClient();

            dynamic parameters = new ExpandoObject();
            parameters.message = txtMessage.Text;

            try
            {
                dynamic id = fb.Post("me/feed", parameters);
                lblPostMessageResult.Text = "Message posted successfully";
                txtMessage.Text = string.Empty;
            }
            catch (FacebookApiException ex)
            {
                lblPostMessageResult.Text = ex.Message;
            }
        }
    }
}