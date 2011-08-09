namespace FacebookDemo.Web.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;
    using Facebook.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
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
                    current.AppSecret != "{app secret}" &&
                    current.CanvasUrl != "http://apps.facebook.com/{fix this path}/")
                {
                    isSetup = true;
                }
            }

            if (!isSetup)
            {
                filterContext.Result = View("GettingStarted");
            }

        }

        public ActionResult Index()
        {
            return View();
        }

        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult About()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("me");
            ViewData["Firstname"] = (string)result.first_name;
            ViewData["Lastname"] = (string)result.last_name;

            return View();
        }

        public ActionResult GettingStarted()
        {
            return View();
        }
    }
}