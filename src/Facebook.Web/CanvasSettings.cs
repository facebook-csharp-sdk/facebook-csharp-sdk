using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Facebook.Web
{
    public class CanvasSettings : ICanvasSettings
    {
        public Uri CanvasPageUrl { get; set; }

        public Uri AuthorizeCancelUrl { get; set; }

        private static ICanvasSettings current;

        /// <summary>
        /// Gets the FacebookSettings stored in the configuration file.
        /// </summary>
        public static ICanvasSettings Current
        {
            get
            {
                if (current == null)
                {
                    var settings = ConfigurationManager.GetSection("canvasSettings");
                    if (settings == null)
                    {
                        throw new ConfigurationErrorsException("Canvas settings section not found in configuration file.");
                    }
                    current = settings as CanvasConfigurationSettings;
                }
                return current;
            }
        }
    }
}
