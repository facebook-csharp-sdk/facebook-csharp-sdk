// --------------------------------
// <copyright file="FacebookSignedRequest.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Rerpesents a Facebook signed request.
    /// </summary>
    public class FacebookSignedRequest
    {

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();


        /// <summary>
        /// Gets the underlying dictionary store.
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
                if (dictionary.ContainsKey("user_id"))
                {
                    return long.Parse(dictionary["user_id"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                dictionary["user_id"] = value.ToString(CultureInfo.InvariantCulture);
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
        /// Gets or sets the profile id.
        /// </summary>
        /// <value>The profile id.</value>
        public long ProfileId
        {
            get
            {
                if (dictionary.ContainsKey("profile_id"))
                {
                    return long.Parse(dictionary["profile_id"], CultureInfo.InvariantCulture);
                }
                return default(long);
            }
            set
            {
                dictionary["profile_id"] = value.ToString(CultureInfo.InvariantCulture);
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

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(dictionary != null);
        }

    }
}
