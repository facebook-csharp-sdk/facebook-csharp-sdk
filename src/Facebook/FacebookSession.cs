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

namespace Facebook
{
    public sealed class FacebookSession
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();


        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        public long UserId
        {
            get
            {
                if (dictionary.ContainsKey("uid"))
                {
                    return long.Parse(dictionary["uid"]);
                }
                return default(long);
            }
            set
            {
                dictionary["uid"] = value.ToString();
            }
        }
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
        public DateTime Expires
        {
            get
            {
                if (dictionary.ContainsKey("expires"))
                {
                    return Utilities.UnixDateTime.FromUnixTime(dictionary["expires"]);
                }
                return default(DateTime);
            }
            set
            {
                dictionary["expires"] = Utilities.UnixDateTime.ToUnixTime(value);
            }
        }
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
    }
}
