using System;
using System.Web.Mvc;
using Facebook;
using Facebook.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace FacebookDemo.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [CanvasAuthorize(Perms = "user_about_me")]
        public ActionResult About()
        {
            FacebookApp fbApp = new FacebookApp();
            if (fbApp.Session != null)
            {
                dynamic result = fbApp.Get("me");
                ViewData["Firstname"] = (string)result.first_name;
                ViewData["Lastname"] = (string)result.last_name;
            }

            return View();
        }
    }
}