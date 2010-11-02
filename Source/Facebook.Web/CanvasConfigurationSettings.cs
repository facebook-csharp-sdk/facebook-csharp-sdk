using System;
using System.Configuration;

namespace Facebook.Web
{
    /// <summary>
    /// Represents the cansvas configuration section.
    /// </summary>
    public class CanvasConfigurationSettings : ConfigurationSection, ICanvasSettings
    {

        /// <summary>
        /// The base url of your application on Facebook.
        /// </summary>
        [ConfigurationProperty("canvasPageUrl", IsRequired = true)]
        public Uri CanvasPage
        {
            get { return (Uri)this["canvasPage"]; }
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
