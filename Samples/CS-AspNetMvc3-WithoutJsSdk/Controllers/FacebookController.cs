using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_AspNetMvc3_WithoutJsSdk.Models;
using Facebook;
using System.Web.Security;

namespace CS_AspNetMvc3_WithoutJsSdk.Controllers
{
    public class FacebookController : Controller
    {
        //
        // GET: /Facebook/
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var user = InMemoryUserStore.Get(User.Identity.Name);

                var fb = new FacebookClient(user.AccessToken);
                dynamic me = fb.Get("me");

                ViewBag.name = me.name;
            }
            catch (FacebookApiException)
            {
                FormsAuthentication.SignOut();
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (InMemoryUserStore.Get(User.Identity.Name) == null)
                filterContext.Result = new HttpUnauthorizedResult();
        }

    }
}
