using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Facebook
{
    /// <summary>
    /// Represents the Facebook section in a configuration file.
    /// </summary>
    public sealed class FacebookConfigurationSection : ConfigurationSection, IFacebookApplication
    {

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

        [ConfigurationProperty("siteUrl", IsRequired = false)]
        public string SiteUrl
        {
            get { return (string)this["siteUrl"]; }
            set { this["siteUrl"] = value; }
        }

        [ConfigurationProperty("canvasPage", IsRequired = false)]
        public string CanvasPage
        {
            get { return (string)this["canvasPage"]; }
            set { this["canvasPage"] = value; }
        }

        [ConfigurationProperty("canvasUrl", IsRequired = false)]
        public string CanvasUrl
        {
            get { return (string)this["canvasUrl"]; }
            set { this["canvasUrl"] = value; }
        }
    }
}
