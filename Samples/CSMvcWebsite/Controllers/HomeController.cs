using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using System.Dynamic;
using Facebook.Web;

namespace Facebook.Samples.HelloWorld.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public FacebookSession CurrentSession
        {
            get { return (new Authorizer().Session); }
        }

        public ActionResult Index()
        {
            if (FacebookContext.Current.AppId == "{put your appId here}")
            {
                return View("GettingStarted");
            }

            return View();
        }

        [FacebookAuthorize]
        public ActionResult About()
        {
            var app = new FacebookApp(this.CurrentSession.AccessToken);

            dynamic me = app.Get("me");
            dynamic friends = app.Get("/me/friends");

            dynamic model = new ExpandoObject();
            model.Name = me.name;
            model.FriendCount = friends.data.Count;

            return View(model);
        }

        [FacebookAuthorize(LoginUrl = "/", Permissions = "publish_stream")]
        public ActionResult Publish()
        {
            var app = new FacebookApp(this.CurrentSession.AccessToken);

            dynamic parameters = new ExpandoObject();
            parameters.message = "First wall post!";

            dynamic result = app.Api("/me/feed", parameters, HttpMethod.Post);

            return RedirectToAction("About");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // YOU DONT NEED ANY OF THIS IN YOUR APPLICATION
            // THIS METHOD JUST CHECKS TO SEE IF YOU HAVE SETUP
            // THE SAMPLE. IF THE SAMPLE IS NOT SETUP YOU WILL
            // BE SENT TO THE GETTING STARTED PAGE.

            base.OnActionExecuting(filterContext);

            bool isSetup = false;
            var settings = ConfigurationManager.GetSection("facebookSettings");
            if (settings != null)
            {
                var current = settings as IFacebookApplication;
                if (current.AppId != "{app id}" &&
                    current.AppSecret != "{app secret}")
                {
                    isSetup = true;
                }
            }

            if (!isSetup)
            {
                filterContext.Result = View("GettingStarted");
            }

        }
    }
}
