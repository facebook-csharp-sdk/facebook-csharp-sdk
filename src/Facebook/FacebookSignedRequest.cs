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
    public class FacebookSignedRequest
    {

        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();


        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this._dictionary;
            }
        }

        /// <summary>
        /// Which user is currently viewing your application.
        /// </summary>
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
        /// The users access token.
        /// </summary>
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
        /// When the access token expires.
        /// </summary>
        public DateTime Expires
        {
            get
            {
                if (_dictionary.ContainsKey("expires") && !String.IsNullOrEmpty(_dictionary["expires"]))
                {
                    return DateTimeUtils.FromUnixTime(_dictionary["expires"]);
                }
                return default(DateTime);
            }
            set
            {
                Contract.Requires(value >= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                _dictionary["expires"] = DateTimeUtils.ToUnixTime(value);
            }
        }
        /// <summary>
        /// Whose profile is currently being viewed. It could be a user ID or a Page ID.
        /// </summary>
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
