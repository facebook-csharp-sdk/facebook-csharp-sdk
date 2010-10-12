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
    public sealed class FacebookSession
    {
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();


        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this._dictionary;
            }
        }

        public long UserId
        {
            get
            {
                if (_dictionary.ContainsKey("uid"))
                {
                    return long.Parse(_dictionary["uid"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                _dictionary["uid"] = value.ToString(CultureInfo.InvariantCulture);
            }
        }
        public string Secret
        {
            get
            {
                if (_dictionary.ContainsKey("secret"))
                {
                    return _dictionary["secret"];
                }
                return null;
            }
            set
            {
                _dictionary["secret"] = value;
            }
        }
        public string AccessToken
        {
            get
            {
                if (_dictionary.ContainsKey("access_token"))
                {
                    return _dictionary["access_token"];
                }
                return null;
            }
            set
            {
                _dictionary["access_token"] = value;
            }
        }
        public string SessionKey
        {
            get
            {
                if (_dictionary.ContainsKey("session_key"))
                {
                    return _dictionary["session_key"];
                }
                return null;
            }
            set
            {
                _dictionary["session_key"] = value;
            }
        }
        public DateTime Expires
        {
            get
            {
                if (_dictionary.ContainsKey("expires") && !String.IsNullOrEmpty(_dictionary["expires"]))
                {
                    return Utilities.DateTimeUtils.FromUnixTime(_dictionary["expires"]);
                }
                return default(DateTime);
            }
            set
            {
                Contract.Requires(value >= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));

                _dictionary["expires"] = Utilities.DateTimeUtils.ToUnixTime(value);
            }
        }
        public string Signature
        {
            get
            {
                if (_dictionary.ContainsKey("sig"))
                {
                    return _dictionary["sig"];
                }
                return null;
            }
            set
            {
                _dictionary["sig"] = value;
            }
        }
        public string BaseDomain
        {
            get
            {
                if (_dictionary.ContainsKey("base_domain"))
                {
                    return _dictionary["base_domain"];
                }
                return null;
            }
            set
            {
                _dictionary["base_domain"] = value;
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
