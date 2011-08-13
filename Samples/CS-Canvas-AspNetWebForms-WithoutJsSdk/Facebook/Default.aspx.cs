using System;
using System.Configuration;
using System.Dynamic;
using Facebook;
using Facebook.Web;

namespace CS_Canvas_AspNetWebForms_WithoutJsSdk.Facebook
{
    public partial class Default : System.Web.UI.Page
    {
        private CanvasAuthorizer _canvasAuthorizer;
        protected CanvasAuthorizer CanvasAuthorizer
        {
            get
            {
                if (_canvasAuthorizer == null)
                {
                    var extendedPermissions = ConfigurationManager.AppSettings["extendedPermissions"].Split(',');
                    _canvasAuthorizer = new CanvasAuthorizer { Permissions = extendedPermissions };
                }
                return _canvasAuthorizer;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CanvasAuthorizer.Authorize())
            {
                var fb = new FacebookWebClient();
                dynamic me = fb.Get("me");

                imgProfilePic.ImageUrl = string.Format("https://graph.facebook.com/{0}/picture", me.id);

                lblName.Text = me.name;
                lblFirstName.Text = me.first_name;
                lblLastName.Text = me.last_name;
            }
        }

        protected void btnPostToWall_Click(object sender, EventArgs e)
        {
            if (CanvasAuthorizer.Authorize())
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
}