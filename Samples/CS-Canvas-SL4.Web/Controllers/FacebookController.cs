using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;

namespace CS_Canvas_SL4.Web.Controllers
{
    public class FacebookController : Controller
    {
        private const string ExtendedPermissions = "user_about_me,publish_stream";

        //
        // GET: /Facebook/
        [CanvasAuthorize(Permissions = ExtendedPermissions)]
        public ActionResult Index()
        {
            ViewBag.AccessToken = FacebookWebContext.Current.AccessToken;

            return View();
        }

    }
}
