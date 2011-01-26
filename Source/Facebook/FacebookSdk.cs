namespace Facebook
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the Facebook sdk class.
    /// </summary>
    public class FacebookSdk
    {
        /// <summary>
        /// Collection for facebook application settings.
        /// </summary>
        private static readonly FacebookAppSettingsCollection ApplicationSettingsCollection;

        /// <summary>
        /// Initializes static members of the <see cref="FacebookSdk"/> class.
        /// </summary>
        static FacebookSdk()
        {
            ApplicationSettingsCollection = new FacebookAppSettingsCollection();

#if !SILVERLIGHT
            var facebookConfigSection = (NFacebookConfigurationSection)System.Configuration.ConfigurationManager.GetSection("facebook");
            if (facebookConfigSection != null && facebookConfigSection.Apps != null)
            {
                foreach (FacebookApplicationSettingsConfigElement app in facebookConfigSection.Apps)
                {
                    ApplicationSettingsCollection.Register(app.AppName, app);
                }
            }
#endif
        }

        /// <summary>
        /// Gets the facebook applications.
        /// </summary>
        public static FacebookAppSettingsCollection Applications
        {
            get
            {
                Contract.Ensures(Contract.Result<FacebookAppSettingsCollection>() != null);
                return ApplicationSettingsCollection;
            }
        }

        /// <summary>
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
            "SA1606:ElementDocumentationMustHaveSummaryText", Justification = "Reviewed. Suppression is OK here."),
         ContractInvariantMethod]
        private static void InvariantObject()
        {
            Contract.Invariant(ApplicationSettingsCollection != null);
        }
    }
}