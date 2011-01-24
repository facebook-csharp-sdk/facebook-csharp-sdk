namespace Facebook.Web.Mvc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    /// <summary>
    /// Represents the class for specifiying the facebook application name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FacebookAppAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// The application name.
        /// </summary>
        private readonly string appName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAppAttribute"/> class.
        /// </summary>
        /// <param name="appName">
        /// The app name.
        /// </param>
        public FacebookAppAttribute(string appName)
        {
            Contract.Requires(!string.IsNullOrEmpty(appName));

            this.appName = appName;
            
            // This order should always be higher than other Facebook attributes
            // like [CanvasAuthorizer] and so on.
            this.Order = -1;
        }

        /// <summary>
        /// Gets the facebook application name.
        /// </summary>
        public string AppName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return this.appName;
            }
        }

        /// <summary>
        /// Gets the facebook application settings.
        /// </summary>
        /// <remarks>
        /// Note: Should return null if the IFacebookAppSettings does not exist.
        /// </remarks>
        public virtual IFacebookAppSettings Settings
        {
            get { return FacebookSdk.Applications[this.AppName]; }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Contract.Requires(filterContext != null);
            Contract.Requires(filterContext.Controller != null);
            Contract.Requires(filterContext.Controller.ViewData != null);

            // save the settings in ViewData, so the next Facebook attributes like
            // [CanvasAuthorize] attributes can make use of it.
            // note: settings can be null if app is not found.
            // if this view data is null then attributes like [CanvasAuthorize] should
            // throw error reporing Facebook Application not configured or some
            // other usefull informations.
            // this FacebookAppAttribute should not throw error coz, there might be
            // some action methods where there is no need to access Facebook stuffs.
            filterContext.Controller.ViewData["facebooksdk-appsettings"] = this.Settings;
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here.")]
        private void InvariantObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(this.appName));
        }
    }
}