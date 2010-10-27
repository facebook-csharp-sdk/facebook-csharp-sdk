using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace Facebook.Samples.HelloWorld.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (FacebookSettings.Current.AppId == "your_app_id")
            {
                return View("GettingStarted");
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var app = new FacebookApp();
            if (app.Session == null)
            {
                // The user isnt logged in to Facebook
                // send them to the home page
                return RedirectToAction("Index");
            }

            // Get the user info from the Graph API
            dynamic me = app.Api("/me");
            ViewData["FirstName"] = me.first_name;
            ViewData["LastName"] = me.last_name;

            return View();
        }
    }
}
