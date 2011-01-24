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
    public class FacebookAppAttribute : ActionFilterAttribute
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
            this.Order = 10;
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
        /// Should return null if the IFacebookAppSettings does not exist.
        /// </remarks>
        public virtual IFacebookAppSettings Settings
        {
            get { return FacebookSdk.Applications[this.AppName]; }
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