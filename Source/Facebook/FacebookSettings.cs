// --------------------------------
// <copyright file="FacebookSettings.cs" company="Facebook C# SDK">
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
    /// Represents the settings of a Facebook application.
    /// </summary>
    public class FacebookSettings : IFacebookSettings
    {
#if !SILVERLIGHT // Silverlight does not support System.Configuration

        /// <summary>
        /// The Facebook settings stored in the configuration file.
        /// </summary>
        private static IFacebookSettings current;

        /// <summary>
        /// Gets the Facebook settings stored in the configuration file.
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

        /// <summary>
        /// Gets or sets the Application secret.
        /// </summary>
        /// <value>The Application secret.</value>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the max retries.
        /// </summary>
        /// <value>The max retries.</value>
        public int MaxRetries { get; set; }

        /// <summary>
        /// Gets or sets the retry delay.
        /// </summary>
        /// <value>The retry delay.</value>
        public int RetryDelay { get; set; }

    }
}
