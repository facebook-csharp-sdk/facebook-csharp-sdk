// --------------------------------
// <copyright file="FacebookConfigurationSection.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System.Configuration;

    /// <summary>
    /// Represents the Facebook section in a configuration file.
    /// </summary>
    public sealed class FacebookConfigurationSection : ConfigurationSection, IFacebookApplication
    {
        /// <summary>
        /// The current Facebook settings stored in the configuration file.
        /// </summary>
        private static IFacebookApplication _current;

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
        /// Gets or sets the API secret.
        /// </summary>
        /// <value>The API secret.</value>
        [ConfigurationProperty("appSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["appSecret"]; }
            set { this["appSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets the site url.
        /// </summary>
        [ConfigurationProperty("siteUrl", IsRequired = false)]
        public string SiteUrl
        {
            get { return (string)this["siteUrl"]; }
            set { this["siteUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the canvas page.
        /// </summary>
        [ConfigurationProperty("canvasPage", IsRequired = false)]
        public string CanvasPage
        {
            get { return (string)this["canvasPage"]; }
            set { this["canvasPage"] = value; }
        }

        /// <summary>
        /// Gets or sets the canvas url.
        /// </summary>
        [ConfigurationProperty("canvasUrl", IsRequired = false)]
        public string CanvasUrl
        {
            get { return (string)this["canvasUrl"]; }
            set { this["canvasUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the secure canvas url.
        /// </summary>
        [ConfigurationProperty("secureCanvasUrl", IsRequired = false)]
        public string SecureCanvasUrl
        {
            get { return (string)this["secureCanvasUrl"]; }
            set { this["secureCanvasUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the url to return the user after they cancel authorization.
        /// </summary>
        [ConfigurationProperty("cancelUrlPath", IsRequired = false)]
        public string CancelUrlPath
        {
            get { return (string)this["cancelUrlPath"]; }
            set { this["cancelUrlPath"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use Facebook beta.
        /// </summary>
        [ConfigurationProperty("useFacebookBeta", IsRequired = false, DefaultValue = false)]
        public bool UseFacebookBeta
        {
            get { return (bool)this["useFacebookBeta"]; }
            set { this["useFacebookBeta"] = value; }
        }

        [ConfigurationProperty("isSecureConnection", IsRequired = false, DefaultValue = false)]
        public bool IsSecureConnection
        {
            get { return (bool)this["isSecureConnection"]; }
            set { this["isSecureConnection"] = value; }
        }

        /// <summary>
        /// Gets the Facebook settings stored in the configuration file.
        /// </summary>
        internal static IFacebookApplication Current
        {
            get
            {
                if (_current == null)
                {
                    var settings = ConfigurationManager.GetSection("facebookSettings");
                    if (settings != null)
                    {
                        _current = settings as IFacebookApplication;
                    }
                }

                return _current;
            }
        }

    }
}
