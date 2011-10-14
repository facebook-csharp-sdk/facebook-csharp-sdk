// --------------------------------
// <copyright file="CanvasSoftAuthorizeAttribute.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Facebook;
    using Facebook.Web;

    /// <summary>
    /// This filter will send an unauthorized user to the 
    /// specified view rather than redirecting them directly
    /// to the Facebook login page. This allows for a landing
    /// page to explain to the user why the permissions requested
    /// are needed.
    /// </summary>
    public class CanvasSoftAuthorizeAttribute : CanvasAuthorizeAttribute
    {
        /// <summary>
        /// The default view.
        /// </summary>
        private const string DefaultView = "FacebookAuthorize";

        /// <summary>
        /// The view name.
        /// </summary>
        private string _view;

        /// <summary>
        /// The master.
        /// </summary>
        private string _master;

        /// <summary>
        /// Gets or sets the name of the View.
        /// </summary>
        public virtual string View
        {
            get { return !string.IsNullOrEmpty(_view) ? _view : DefaultView; }
            set { _view = value; }
        }

        /// <summary>
        /// Gets or sets the Master.
        /// </summary>
        public virtual string Master
        {
            get { return _master ?? string.Empty; }
            set { _master = value; }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext, IFacebookApplication settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            var model = new FacebookAuthorizeInfo(
                GetLoginUrl(settings, filterContext.HttpContext, null),
                Permissions,
                filterContext.HttpContext.Request.QueryString.AllKeys.Contains("error_reason"),
                filterContext.RouteData.Values);

            var viewResult = new ViewResult
                                       {
                                           MasterName = Master,
                                           ViewName = View,
                                           ViewData = new ViewDataDictionary<FacebookAuthorizeInfo>(model),
                                           TempData = filterContext.Controller.TempData
                                       };

            filterContext.Result = viewResult;
        }

        protected internal override Uri GetLoginUrl(IFacebookApplication settings, HttpContextBase httpContext, IDictionary<string, object> parameters)
        {
            var authorizer = new CanvasAuthorizer(settings, httpContext)
            {
                ReturnUrlPath = ReturnUrlPath,
                CancelUrlPath = CancelUrlPath,
                LoginDisplayMode = LoginDisplayMode
            };

            if (!String.IsNullOrEmpty(Permissions))
            {
                authorizer.Permissions = Permissions.Replace(" ", String.Empty).Split(',');
            }

            if (string.IsNullOrEmpty(CancelUrlPath))
            {
                // set it to this same url instead of going to facebook.com
                var canvasUrlBuilder = new CanvasUrlBuilder(settings, httpContext.Request);
                var currentPathAndQuery = canvasUrlBuilder.CurrentCanvasPathAndQuery;

                if (currentPathAndQuery.Contains("?"))
                {
                    var parts = currentPathAndQuery.Split('?');
                    if (parts.Length == 2 && !string.IsNullOrEmpty(parts[1]))
                    {
                        var queryStrings = FacebookUtils.ParseUrlQueryString(parts[1]);

                        // remove oauth 2 error querystrings.
                        // error_reason=user_denied&error_denied=access_denied&error_description=The+user+denied+your+request.
                        if (queryStrings.ContainsKey("error_reason"))
                        {
                            queryStrings.Remove("error_reason");
                        }

                        if (queryStrings.ContainsKey("error_denied"))
                        {
                            queryStrings.Remove("error_denied");
                        }

                        if (queryStrings.ContainsKey("error_description"))
                        {
                            queryStrings.Remove("error_description");
                        }

                        currentPathAndQuery = parts[0] + "?" + FacebookUtils.ToJsonQueryString(queryStrings);
                    }
                }

                authorizer.CancelUrlPath = currentPathAndQuery;
            }

            return authorizer.GetLoginUrl(null);
        }
    }
}