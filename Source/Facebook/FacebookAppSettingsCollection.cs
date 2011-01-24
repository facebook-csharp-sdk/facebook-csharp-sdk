namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a collection of Facebook application settings.
    /// </summary>
    public class FacebookAppSettingsCollection
    {
        /// <summary>
        /// List of facebook applications.
        /// </summary>
        private readonly Dictionary<string, IFacebookAppSettings> facebookAppSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAppSettingsCollection"/> class.
        /// </summary>
        public FacebookAppSettingsCollection()
        {
            this.facebookAppSettings = new Dictionary<string, IFacebookAppSettings>();
        }

        /// <summary>
        /// Indexer for Facebook application settings collection.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        public IFacebookAppSettings this[string appId]
        {
            get
            {
                return this.facebookAppSettings[appId];
            }

            set
            {
                if (value == null)
                {
                    if (this.facebookAppSettings.ContainsKey(appId))
                    {
                        this.facebookAppSettings.Remove(appId);
                    }
                }
                else
                {
                    this.facebookAppSettings[appId] = value;
                }
            }
        }

        /// <summary>
        /// Adds the facebook application settings to the collection.
        /// </summary>
        /// <param name="facebookAppSettings">
        /// The facebook app settings.
        /// </param>
        public void Add(IFacebookAppSettings facebookAppSettings)
        {
            Contract.Requires(facebookAppSettings != null);
            Contract.Requires(!string.IsNullOrEmpty(facebookAppSettings.AppId));

            if (this.facebookAppSettings.ContainsKey(facebookAppSettings.AppId))
            {
                throw new ArgumentException("Facebook application with the same appid already exists");
            }

            this.facebookAppSettings.Add(facebookAppSettings.AppId, facebookAppSettings);
        }

        /// <summary>
        /// Remove the facebook application settings from the collection.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        public void Remove(string appId)
        {
            Contract.Requires(!string.IsNullOrEmpty(appId));
            if (!this.facebookAppSettings.ContainsKey(appId))
            {
                throw new ArgumentException("Facebook application with the specified app id does not exist.", "appid");
            }
            
            this.facebookAppSettings.Remove(appId);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this.facebookAppSettings != null);
        }
    }
}