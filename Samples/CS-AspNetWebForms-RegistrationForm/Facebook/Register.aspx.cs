using System;
using System.Web;
using Facebook;

namespace CS_AspNetWebForms_RegistrationForm.Facebook
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string RegistrationUrl
        {
            get
            {
                var fields = new object[]
                                 {
                                     new {name = "name"},
                                     new {name = "email"},
                                     new {name = "location"},
                                     new {name = "gender"},
                                     new {name = "birthday"},
                                     new {name = "password", view = "not_prefilled"},
                                     new
                                         {
                                             name = "like",
                                             description = "Do you like this plugin?",
                                             type = "checkbox",
                                             @default = "checked"
                                         },
                                     new {name = "phone", description = "Phone Number", type = "text"},
                                     new {name = "captcha"}
                                 };

                return string.Format(
                        "http://www.facebook.com/plugins/registration.php?client_id={0}&redirect_uri={1}&fields={2}",
                        FacebookApplication.Current.AppId,
                        HttpUtility.UrlEncode("http://localhost:9242/Facebook/RegistrationCallback.aspx"),
                        HttpUtility.UrlEncode(JsonSerializer.Current.SerializeObject(fields)));
            }
        }
    }
}