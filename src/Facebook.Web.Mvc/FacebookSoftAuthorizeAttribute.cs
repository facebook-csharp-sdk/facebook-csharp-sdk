using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facebook.Web.Mvc
{
    /// <summary>
    /// This filter will send an unauthorized user to the 
    /// specified view rather than redirecting them directly
    /// to the facebook login page. This allows for a landing
    /// page to explain to the user why the permissions requested
    /// are needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class FacebookSoftAuthorizeAttribute : FacebookAuthorizeAttribute
    {
        private const string _defaultView = "FacebookAuthorize";
        private string _view;
        private string _master;

        public string View
        {
            get
            {
                return (!String.IsNullOrEmpty(_view)) ? _view : _defaultView;
            }
            set
            {
                _view = value;
            }
        }

        public string Master
        {
            get
            {
                return _master ?? String.Empty;
            }
            set
            {
                _master = value;
            }
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var model = new FacebookAuthorizeInfo
            {
                AuthorizeUrl = GetLoginUrl(filterContext, true /* cancel to self */),
                Perms = this.Perms,
                IsCancelReturn = filterContext.HttpContext.Request.QueryString.AllKeys.Contains("cancel"),
                RouteValues = filterContext.RouteData.Values ?? new RouteValueDictionary(),
            };
            filterContext.Result = new ViewResult
            {
                ViewName = View,
                MasterName = Master,
                ViewData = new ViewDataDictionary<FacebookAuthorizeInfo>(model),
                TempData = filterContext.Controller.TempData
            };
        }
    }
}
