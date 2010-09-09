// --------------------------------
// <copyright file="FacebookSettings.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Configuration;

namespace Facebook
{
    public class FacebookSettings : IFacebookSettings
    {

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public string AppId { get; set; }

        public bool CookieSupport { get; set; }

        public string BaseDomain { get; set; }

        public int MaxRetries { get; set; }

        public int RetryDelay { get; set; }

#if (!SILVERLIGHT) // Silverlight does not support System.Configuration
        private static IFacebookSettings current;

        /// <summary>
        /// Gets the FacebookSettings stored in the configuration file.
        /// </summary>
        public static IFacebookSettings Current
        {
            get
            {
                if (current == null)
                {
                    var settings = ConfigurationManager.GetSection("facebookSettings");
                    if (settings != null)
                    {
                        current = settings as FacebookConfigurationSection;
                    }
                }
                return current;
            }
        }
#endif

    }
}
