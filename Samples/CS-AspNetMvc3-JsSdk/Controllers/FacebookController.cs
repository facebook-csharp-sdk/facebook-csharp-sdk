using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Facebook;
using Facebook.Web;
using Facebook.Web.Mvc;

namespace CS_AspNetMvc3_JsSdk.Controllers
{
    public class FacebookController : Controller
    {
        private const string ExtendedPermissions = "user_about_me,publish_stream";

        //
        // GET: /Facebook/
        [FacebookAuthorize(Permissions = ExtendedPermissions, LoginUrl = "/Facebook/LogOn?ReturnUrl=~/Facebook")]
        public ActionResult Index()
        {
            var fb = new FacebookWebClient();
            dynamic me = fb.Get("me");

            ViewBag.ProfilePictureUrl = string.Format("http://graph.facebok.com/{0}/picture", me.id);
            ViewBag.Name = me.name;
            ViewBag.FirstName = me.first_name;
            ViewBag.LastName = me.last_name;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }

        //
        // POST: /Facebook/MessagePost
        [HttpPost]
        [FacebookAuthorize(Permissions = ExtendedPermissions, LoginUrl = "/Facebook/LogOn?ReturnUrl=~/Facebook")]
        public ActionResult MessagePost(string message)
        {
            var fb = new FacebookWebClient();

            dynamic parameters = new ExpandoObject();
            parameters.message = message;

            dynamic result = fb.Post("me/feed", parameters);

            return RedirectToAction("Index", new { success = true });
        }

        //
        // GET: /Facebook/LogOn
        public ActionResult LogOn(string returnUrl)
        {
            var fbWebContext = new FacebookWebContext(FacebookApplication.Current, ControllerContext.HttpContext); // or FacebookWebContext.Current;

            if (fbWebContext.IsAuthorized(ExtendedPermissions.Split(',')))
            {
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return new RedirectResult(returnUrl);
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ExtendedPermissions = ExtendedPermissions;
            return View();
        }

    }
}