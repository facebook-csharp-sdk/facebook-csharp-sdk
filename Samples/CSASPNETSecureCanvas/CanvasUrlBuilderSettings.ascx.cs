using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Facebook.Web;

namespace CSASPNETSecureCanvas
{
    public partial class CanvasUrlBuilderSettings : System.Web.UI.UserControl
    {
        protected CanvasUrlBuilder CanvasUrlBuilder;

        protected void Page_Load(object sender, EventArgs e)
        {
            CanvasUrlBuilder = new CanvasUrlBuilder(FacebookApplication.Current, new HttpRequestWrapper(Request));

            var client = new FacebookWebClient();
            PicUrlWebClient = ((dynamic)client.Get("/4", new Dictionary<string, object> { { "fields", "picture" } })).picture;

            var app = new FacebookApp();
            PicUrlApp = ((dynamic)app.Get("/4", new Dictionary<string, object> { { "fields", "picture" } })).picture;
        }

        public string PicUrlWebClient = null;
        public string PicUrlApp = null;

    }
}