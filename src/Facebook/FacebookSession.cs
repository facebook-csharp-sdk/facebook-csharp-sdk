// --------------------------------
// <copyright file="FacebookSession.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Facebook
{
    /// <summary>
    /// Represents a Facebook session.
    /// </summary>
    public sealed class FacebookSession
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        /// <summary>
        /// Gets the internal dictionary store.
        /// </summary>
        /// <value>The dictionary.</value>
        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public long UserId
        {
            get
            {
                if (dictionary.ContainsKey("uid"))
                {
                    return long.Parse(dictionary["uid"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                dictionary["uid"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }
        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        public string Secret
        {
            get
            {
                if (dictionary.ContainsKey("secret"))
                {
                    return dictionary["secret"];
                }
                return null;
            }
            set
            {
                dictionary["secret"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken
        {
            get
            {
                if (dictionary.ContainsKey("access_token"))
                {
                    return dictionary["access_token"];
                }
                return null;
            }
            set
            {
                dictionary["access_token"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        /// <value>The session key.</value>
        public string SessionKey
        {
            get
            {
                if (dictionary.ContainsKey("session_key"))
                {
                    return dictionary["session_key"];
                }
                return null;
            }
            set
            {
                dictionary["session_key"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires
        {
            get
            {
                if (dictionary.ContainsKey("expires") && !String.IsNullOrEmpty(dictionary["expires"]))
                {
                    return DateTimeConvertor.FromUnixTime(dictionary["expires"]);
                }
                return default(DateTime);
            }
            set
            {
                Contract.Requires(value >= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

                dictionary["expires"] = DateTimeConvertor.ToUnixTime(value).ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature
        {
            get
            {
                if (dictionary.ContainsKey("sig"))
                {
                    return dictionary["sig"];
                }
                return null;
            }
            set
            {
                dictionary["sig"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the base domain.
        /// </summary>
        /// <value>The base domain.</value>
        public string BaseDomain
        {
            get
            {
                if (dictionary.ContainsKey("base_domain"))
                {
                    return dictionary["base_domain"];
                }
                return null;
            }
            set
            {
                dictionary["base_domain"] = value;
            }
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(dictionary != null);
        }
    }
}
