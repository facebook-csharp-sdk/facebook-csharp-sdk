using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Configuration;

namespace Facebook.Samples.MvcWebsite.Controllers
{

    [HandleError]
    public class FacebookController : Controller {

        public PartialViewResult InitScript() {
            FacebookApp app = new FacebookApp();
            dynamic model = new ExpandoObject();
            model.AppId = app.AppId;
            model.CookieSupport = app.CookieSupport;
            return PartialView(model);
        }

        public PartialViewResult ProfileBox()
        {
            FacebookApp app = new FacebookApp();
            if (app.Session == null)
            {
                return PartialView("LoginButton");
            }
            else
            {
                ViewData["UserId"] = app.UserId;
                return PartialView("ProfileBox");
            }
        }

    }
}
