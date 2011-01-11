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
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Rerpesents a Facebook signed request.
    /// </summary>
    public class FacebookSignedRequest : JObject
    {

        public FacebookSignedRequest(JObject other)
            : base(other)
        {
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public long UserId
        {
            get
            {

                if (this["user_id"] != null)
                {
                    var s = this.Value<string>("user_id");
                    return long.Parse(s, CultureInfo.InvariantCulture);
                }
                return default(long);
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
                return this.Value<string>("oauth_token");
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
                if (this["expires"] != null)
                {
                    var s = this.Value<string>("expires");
                    if (!String.IsNullOrEmpty(s))
                    {
                        return FacebookUtils.FromUnixTime(s);
                    }
                }
                return default(DateTime);
            }
        }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime IssuedAt
        {
            get
            {
                if (this["issued_at"] != null)
                {
                    var s = this.Value<string>("issued_at");
                    if (!String.IsNullOrEmpty(s))
                    {
                        return FacebookUtils.FromUnixTime(s);
                    }
                }
                return default(DateTime);
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
                if (this["profile_id"] != null)
                {
                    var s = this.Value<string>("profile_id");
                    return long.Parse(s, CultureInfo.InvariantCulture);
                }
                return default(long);
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
                return this.Value<string>("algorithm");
            }
        }

        public FacebookSignedRequestUser User
        {
            get
            {
                if (this["user"] != null)
                {
                    return new FacebookSignedRequestUser(this.Value<JObject>("user"));
                }
                return null;
            }
        }

    }

    public class FacebookSignedRequestUser : JObject
    {

        public FacebookSignedRequestUser(JObject other)
            : base(other)
        {
        }

        public string Locale
        {
            get
            {
                return this.Value<string>("locale");
            }
        }

        public string Country
        {
            get
            {
                return this.Value<string>("country");
            }
        }
    }
}
