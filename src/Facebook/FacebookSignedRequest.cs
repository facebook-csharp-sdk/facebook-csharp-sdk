// --------------------------------
// <copyright file="FacebookSignedRequest.cs" company="Thuzi, LLC">
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
    /// Rerpesents a Facebook signed request.
    /// </summary>
    public class FacebookSignedRequest
    {

        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();


        /// <summary>
        /// Gets the underlying dictionary store.
        /// </summary>
        /// <value>The dictionary.</value>
        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this._dictionary;
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
                if (_dictionary.ContainsKey("user_id"))
                {
                    return long.Parse(_dictionary["user_id"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                _dictionary["user_id"] = value.ToString(CultureInfo.InvariantCulture);
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
                if (_dictionary.ContainsKey("oauth_token"))
                {
                    return _dictionary["oauth_token"];
                }
                return null;
            }
            set
            {
                _dictionary["oauth_token"] = value;
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
                if (_dictionary.ContainsKey("expires") && !String.IsNullOrEmpty(_dictionary["expires"]))
                {
                    return DateTimeConvertor.FromUnixTime(_dictionary["expires"]);
                }
                return default(DateTime);
            }
            set
            {
                Contract.Requires(value >= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                _dictionary["expires"] = DateTimeConvertor.ToUnixTime(value).ToString(CultureInfo.InvariantCulture);
            }
        }


        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        /// <value>The profile id.</value>
        public long ProfileId
        {
            get
            {
                if (_dictionary.ContainsKey("profile_id"))
                {
                    return long.Parse(_dictionary["profile_id"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                _dictionary["profile_id"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public string Algorithm
        {
            get
            {
                if (_dictionary.ContainsKey("algorithm"))
                {
                    return _dictionary["algorithm"];
                }
                return null;
            }
            set
            {
                _dictionary["algorithm"] = value;
            }
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_dictionary != null);
        }

    }
}
