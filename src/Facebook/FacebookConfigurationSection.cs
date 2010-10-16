// --------------------------------
// <copyright file="FacebookConfigurationSection.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System.Configuration;

namespace Facebook
{
    /// <summary>
    /// Represents the Facebook section in a configuration file.
    /// </summary>
    public sealed class FacebookConfigurationSection : ConfigurationSection, IFacebookSettings
    {

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            get { return (string)this["apiKey"]; }
            set { this["apiKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the API secret.
        /// </summary>
        /// <value>The API secret.</value>
        [ConfigurationProperty("apiSecret", IsRequired = true)]
        public string ApiSecret
        {
            get { return (string)this["apiSecret"]; }
            set { this["apiSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["appId"]; }
            set { this["appId"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cookie support].
        /// </summary>
        /// <value><c>true</c> if [cookie support]; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("cookieSupport", IsRequired = false, DefaultValue = false)]
        public bool CookieSupport
        {
            get { return (bool)this["cookieSupport"]; }
            set { this["cookieSupport"] = value; }
        }

        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        [ConfigurationProperty("baseDomain", IsRequired = false)]
        public string BaseDomain
        {
            get { return (string)this["baseDomain"]; }
            set { this["baseDomain"] = value; }
        }

        /// <summary>
        /// Gets or sets the max retries.
        /// </summary>
        /// <value>The max retries.</value>
        [ConfigurationProperty("maxRetries", IsRequired = false, DefaultValue = -1)]
        public int MaxRetries
        {
            get { return (int)this["maxRetries"]; }
            set { this["maxRetries"] = value; }
        }

        /// <summary>
        /// Gets or sets the retry delay.
        /// </summary>
        /// <value>The retry delay.</value>
        [ConfigurationProperty("retryDelay", IsRequired = false, DefaultValue = -1)]
        public int RetryDelay
        {
            get { return (int)this["retryDelay"]; }
            set { this["retryDelay"] = value; }
        }
    }
}
