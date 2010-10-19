// --------------------------------
// <copyright file="IFacebookSettings.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    /// <summary>
    /// Represents the settings of a Facebook application.
    /// </summary>
    public interface IFacebookSettings
    {

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        string ApiKey { get; set; }
        /// <summary>
        /// Gets or sets the API secret.
        /// </summary>
        /// <value>The API secret.</value>
        string ApiSecret { get; set; }
        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        string AppId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [cookie support].
        /// </summary>
        /// <value><c>true</c> if [cookie support]; otherwise, <c>false</c>.</value>
        bool CookieSupport { get; set; }
        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        string BaseDomain { get; set; }
        /// <summary>
        /// Gets or sets the max retries.
        /// </summary>
        /// <value>The max retries.</value>
        int MaxRetries { get; set; }
        /// <summary>
        /// Gets or sets the retry delay.
        /// </summary>
        /// <value>The retry delay.</value>
        int RetryDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attempts to access invalid properties should be traced as warnings.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if attempts to access invalid properties should be traced as warnings; otherwise, <c>false</c>.
        /// </value>
        //bool TraceInvalidProperties { get; set; }
    }
}
