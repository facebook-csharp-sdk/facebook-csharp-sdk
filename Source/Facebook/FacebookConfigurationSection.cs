// --------------------------------
// <copyright file="FacebookConfigurationSection.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Represents the Facebook section in a configuration file.
    /// </summary>
    public sealed class FacebookConfigurationSection : ConfigurationSection, IFacebookSettings
    {
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

        /// <summary>
        /// The base url of your application on Facebook.
        /// </summary>
        [ConfigurationProperty("canvasPageUrl", IsRequired = true)]
        public Uri CanvasPageUrl
        {
            get { return (Uri)this["canvasPageUrl"]; }
            set { this["canvasPageUrl"] = value; }
        }

        /// <summary>
        /// Facebook pulls the content for your application's 
        /// canvas pages from this base url.
        /// </summary>
        [ConfigurationProperty("canvasUrl", IsRequired = false, DefaultValue = null)]
        public Uri CanvasUrl
        {
            get { return (Uri)this["canvasUrl"]; }
            set { this["canvasUrl"] = value; }
        }

        /// <summary>
        /// The url to return the user after they
        /// cancel authorization.
        /// </summary>
        [ConfigurationProperty("authorizeCancelUrl", IsRequired = false, DefaultValue = null)]
        public Uri AuthorizeCancelUrl
        {
            get { return (Uri)this["authorizeCancelUrl"]; }
            set { this["authorizeCancelUrl"] = value; }
        }
    }
}
