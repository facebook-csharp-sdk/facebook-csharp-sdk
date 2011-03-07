using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Facebook.Web
{
    /// <summary>
    /// Represents the cansvas configuration section.
    /// </summary>
    [Obsolete("Use Facebook.FacebookConfigurationSettings")]
    [TypeForwardedFrom("Facebook.Web, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public class CanvasConfigurationSettings : ConfigurationSection, ICanvasSettings
    {

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
