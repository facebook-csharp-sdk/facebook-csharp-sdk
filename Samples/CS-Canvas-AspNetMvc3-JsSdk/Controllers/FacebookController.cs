using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;

namespace CS_Canvas_AspNetMvc3_JsSdk.Controllers
{
    public class FacebookController : Controller
    {
        private const string ExtendedPermissions = "user_about_me,publish_stream";

        //
        // GET: /Facebook/
        [CanvasSoftAuthorize(View = "FacebookLogin", Permissions = ExtendedPermissions)]
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

        [HttpPost]
        [CanvasSoftAuthorize(View = "FacebookLogin", Permissions = ExtendedPermissions)]
        public ActionResult MessagePost(string message)
        {
            var fb = new FacebookWebClient();

            dynamic parameters = new ExpandoObject();
            parameters.message = message;

            dynamic result = fb.Post("me/feed", parameters);

            return RedirectToAction("Index", new { success = true });
        }

    }
}
