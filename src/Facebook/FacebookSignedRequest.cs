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

namespace Facebook
{
    public class FacebookSignedRequest
    {

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();


        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        /// <summary>
        /// Which user is currently viewing your application.
        /// </summary>
        public long UserId
        {
            get
            {
                if (dictionary.ContainsKey("user_id"))
                {
                    return long.Parse(dictionary["user_id"]);
                }
                return default(long);
            }
            set
            {
                dictionary["user_id"] = value.ToString();
            }
        }

        /// <summary>
        /// The users access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (dictionary.ContainsKey("oauth_token"))
                {
                    return dictionary["oauth_token"];
                }
                return null;
            }
            set
            {
                dictionary["oauth_token"] = value;
            }
        }

        /// <summary>
        /// When the access token expires.
        /// </summary>
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
        /// <summary>
        /// Whose profile is currently being viewed. It could be a user ID or a Page ID.
        /// </summary>
        public long ProfileId
        {
            get
            {
                if (dictionary.ContainsKey("profile_id"))
                {
                    return long.Parse(dictionary["profile_id"]);
                }
                return default(long);
            }
            set
            {
                dictionary["profile_id"] = value.ToString();
            }
        }

        public string Algorithm
        {
            get
            {
                if (dictionary.ContainsKey("algorithm"))
                {
                    return dictionary["algorithm"];
                }
                return null;
            }
            set
            {
                dictionary["algorithm"] = value;
            }
        }

    }
}
