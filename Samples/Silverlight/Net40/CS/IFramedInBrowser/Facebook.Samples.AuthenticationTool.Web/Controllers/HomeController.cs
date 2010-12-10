using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Facebook.Web.Mvc;

namespace IFramedInBrowser.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [CanvasAuthorize(Perms = "user_about_me")]
        public ActionResult Index()
        {
            IFramedInBrowser.Web.Models.FacebookToSilverlightViewModel model = null;
            FacebookApp fbApp = new FacebookApp();
            if (fbApp.Session != null && !String.IsNullOrEmpty(fbApp.Session.AccessToken))
            {
                // Be cautious with this value. You might want to consider not
                // letting Silverlight even have this value and have it make
                // service calls and store this value in session.
                model = new IFramedInBrowser.Web.Models.FacebookToSilverlightViewModel { FbSessionKey = fbApp.Session.AccessToken };
            }
            return View(model);
        }
    }
}