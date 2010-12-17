using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Facebook.Web.Mvc;
using IFramedInBrowser.Web.Models;

namespace IFramedInBrowser.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [CanvasAuthorize(Perms = "user_about_me")]
        public ActionResult Index()
        {
            FacebookApp fbApp = new FacebookApp();
            var model = new FacebookToSilverlightViewModel { FbSessionKey = fbApp.Session.AccessToken };
            return View(model);
        }
    }
}