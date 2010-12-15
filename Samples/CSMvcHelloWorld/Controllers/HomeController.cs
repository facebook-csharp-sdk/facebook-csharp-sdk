using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using System.Dynamic;

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

        [CanvasAuthorize(Perms = "stream_publish")]
        public ActionResult Profile()
        {
            var app = new FacebookApp();
            if (app.Session == null)
            {
                // The user isnt logged in to Facebook
                // send them to the home page
                return RedirectToAction("Index");
            }

            dynamic parameters = new ExpandoObject();
            parameters.message = "First wall post!";

            dynamic result = app.Api("/me/feed", parameters, HttpMethod.Post);

            return View();
        }
    }
}
