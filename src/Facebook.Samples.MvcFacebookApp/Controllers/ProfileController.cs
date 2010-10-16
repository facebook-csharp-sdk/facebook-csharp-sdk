// --------------------------------
// <copyright file="ProfileController.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Dynamic;
using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace Facebook.Samples.MvcFacebookApp.Controllers
{
    [HandleError]
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        [CanvasAuthorize(Perms = "email,user_likes")]
        public ActionResult Index(Int64 FacebookId)
        {

            // https://graph.facebook.com/me

            FacebookApp app = new FacebookApp();
            dynamic result = app.Api("me");
            return View("Profile", result);
        }

        [CanvasAuthorize(Perms = "email,user_likes")]
        public ActionResult SpecifyFields()
        {

            // https://graph.facebook.com/me?fields=id,name,picture

            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.fields = "id,name,about";
            dynamic result = app.Api("me", parameters);
            return View("Profile", result);
        }

        [FacebookSoftAuthorize(Perms = "email,user_likes")]
        public ActionResult SoftAuthorize(int id)
        {

            // https://graph.facebook.com/me?fields=id,name,picture

            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.fields = "id,name,about";
            dynamic result = app.Api("me", parameters);
            return View("Profile", result);
        }

        [CanvasAuthorize]
        public ActionResult PostTest()
        {
            return View();
        }

        [HttpPost]
        [CanvasAuthorize]
        public ActionResult PostTest(string test)
        {
            FacebookApp app = new FacebookApp();
            var userId = app.UserId;
            return this.CanvasRedirectToAction("PostTest");
        }

    }
}
