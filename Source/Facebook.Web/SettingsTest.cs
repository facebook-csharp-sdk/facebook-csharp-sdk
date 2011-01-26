using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Facebook.Web
{
    class SettingsTest
    {

        public void Application_Start()
        {
            FacebookContext.SetApplication(new RequestScopedFacebookApplication());
        }


        private class RequestScopedFacebookApplication : IFacebookApplication
        {

            private IFacebookApplication Current
            {
                get
                {
                    var url = HttpContext.Current.Request.Url;
                    var app = GetSettingsForUrl(url);
                    return app;
                }
            }

            private IFacebookApplication GetSettingsForUrl(Uri uri)
            {
                throw new NotImplementedException();
            }

            public string AppId
            {
                get { return Current.AppId; }
            }

            public string AppSecret
            {
                get { return Current.AppSecret; }
            }

            public string SiteUrl
            {
                get { return Current.SiteUrl; }
            }

            public string CanvasPage
            {
                get { return Current.CanvasPage; }
            }

            public string CanvasUrl
            {
                get { return Current.CanvasUrl; }
            }
        }


    }
}
