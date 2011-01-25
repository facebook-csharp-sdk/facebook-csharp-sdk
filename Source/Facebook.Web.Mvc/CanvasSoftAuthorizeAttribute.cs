using System;
using System.Linq;
using System.Web.Mvc;

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class CanvasSoftAuthorizeAttribute : CanvasAuthorizeAttribute
    {
        private const string _defaultView = "FacebookAuthorize";
        private string _view;
        private string _master;

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
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

        /// <summary>
        /// Gets or sets the master.
        /// </summary>
        /// <value>The master.</value>
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

        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="facebookApp">The current Facebook app instance.</param>
        /// <param name="filterContext">The filter context.</param>
        protected override void HandleUnauthorizedRequest(FacebookApp facebookApp, AuthorizationContext filterContext)
        {
            CanvasUrlBuilder urlBuilder = new CanvasUrlBuilder(filterContext.HttpContext.Request);
            var url = urlBuilder.GetLoginUrl(facebookApp, Perms, ReturnUrlPath, CancelUrlPath, true);

            var model = new FacebookAuthorizeInfo(
                url,
                this.Perms,
                filterContext.HttpContext.Request.QueryString.AllKeys.Contains("cancel"),
                filterContext.RouteData.Values);

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
