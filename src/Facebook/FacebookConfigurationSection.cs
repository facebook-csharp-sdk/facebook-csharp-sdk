// --------------------------------
// <copyright file="FacebookConfigurationSection.cs" company="Thuzi, LLC">
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
    public sealed class FacebookConfigurationSection : ConfigurationSection, IFacebookSettings
    {

        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            get { return (string)this["apiKey"]; }
            set { this["apiKey"] = value; }
        }

        [ConfigurationProperty("apiSecret", IsRequired = true)]
        public string ApiSecret
        {
            get { return (string)this["apiSecret"]; }
            set { this["apiSecret"] = value; }
        }

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["appId"]; }
            set { this["appId"] = value; }
        }

        [ConfigurationProperty("cookieSupport", IsRequired = false, DefaultValue = false)]
        public bool CookieSupport
        {
            get { return (bool)this["cookieSupport"]; }
            set { this["cookieSupport"] = value; }
        }

        [ConfigurationProperty("baseDomain", IsRequired = false)]
        public string BaseDomain
        {
            get { return (string)this["baseDomain"]; }
            set { this["baseDomain"] = value; }
        }

        [ConfigurationProperty("maxRetries", IsRequired = false, DefaultValue = -1)]
        public int MaxRetries
        {
            get { return (int)this["maxRetries"]; }
            set { this["maxRetries"] = value; }
        }

        [ConfigurationProperty("retryDelay", IsRequired = false, DefaultValue = -1)]
        public int RetryDelay
        {
            get { return (int)this["retryDelay"]; }
            set { this["retryDelay"] = value; }
        }
    }
}
