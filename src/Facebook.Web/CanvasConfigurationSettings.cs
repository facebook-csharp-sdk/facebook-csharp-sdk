using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Facebook.Web
{
    public class CanvasConfigurationSettings : ConfigurationSection, ICanvasSettings
    {

        /// <summary>
        /// The url to your canvas application.
        /// </summary>
        [ConfigurationProperty("canvasPageUrl", IsRequired = false)]
        public Uri CanvasPageUrl
        {
            get
            {
                if (this.Properties.Contains("canvasPageUrl"))
                {
                    return (Uri)this["canvasPageUrl"];
                }
                return null;
            }
            set { this["canvasPageUrl"] = value; }
        }

        /// <summary>
        /// The url to return the user after they
        /// cancel authorization.
        /// </summary>
        [ConfigurationProperty("authorizeCancelUrl", IsRequired = false)]
        public Uri AuthorizeCancelUrl
        {
            get
            {
                if (this.Properties.Contains("authorizeCancelUrl"))
                {
                    return (Uri)this["authorizeCancelUrl"];
                }
                return null;
            }
            set { this["authorizeCancelUrl"] = value; }
        }
    }
}
