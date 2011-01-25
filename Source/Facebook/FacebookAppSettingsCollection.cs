namespace Facebook
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a collection of Facebook application settings.
    /// </summary>
    public class FacebookAppSettingsCollection : IEnumerable, IEnumerable<KeyValuePair<string, IFacebookAppSettings>>
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
        /// <param name="appName">
        /// The app id.
        /// </param>
        public IFacebookAppSettings this[string appName]
        {
            get
            {
                return this.facebookAppSettings.ContainsKey(appName) ? this.facebookAppSettings[appName] : null;
            }

            set
            {
                if (value == null)
                {
                    if (this.facebookAppSettings.ContainsKey(appName))
                    {
                        this.facebookAppSettings.Remove(appName);
                    }
                }
                else
                {
                    this.facebookAppSettings[appName] = value;
                }
            }
        }

        /// <summary>
        /// Adds the facebook application settings to the collection.
        /// </summary>
        /// <param name="appName">
        /// The app name.
        /// </param>
        /// <param name="facebookAppSettings">
        /// The facebook app settings.
        /// </param>
        public void Register(string appName, IFacebookAppSettings facebookAppSettings)
        {
            Contract.Requires(!string.IsNullOrEmpty(appName));
            Contract.Requires(facebookAppSettings != null);

            if (this.facebookAppSettings.ContainsKey(appName))
            {
                throw new ArgumentException("Facebook application with same name already exists.", "appName");
            }

            this.facebookAppSettings.Add(appName, facebookAppSettings);
        }

        /// <summary>
        /// Remove the facebook application settings from the collection.
        /// </summary>
        /// <param name="appName">
        /// The facebook application name.
        /// </param>
        public void Remove(string appName)
        {
            Contract.Requires(!string.IsNullOrEmpty(appName));

            if (!this.facebookAppSettings.ContainsKey(appName))
            {
                throw new ArgumentException("Facebook application with the specified name does not exist.", "appName");
            }

            this.facebookAppSettings.Remove(appName);
        }

        /// <summary>
        /// Clears the facebook app settings collection.
        /// </summary>
        public void Clear()
        {
            this.facebookAppSettings.Clear();
        }

        IEnumerator<KeyValuePair<string, IFacebookAppSettings>> IEnumerable<KeyValuePair<string, IFacebookAppSettings>>.GetEnumerator()
        {
            return this.facebookAppSettings.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return this.facebookAppSettings.GetEnumerator();
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this.facebookAppSettings != null);
        }
    }
}