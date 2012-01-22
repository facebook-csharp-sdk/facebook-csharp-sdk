// --------------------------------
// <copyright file="FacebookClient.OAuthResult.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Collections.Generic;

    public partial class FacebookClient
    {
        /// <summary>
        /// Try parsing the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="url">The url to parse</param>
        /// <param name="facebookOAuthResult">The facebook oauth result.</param>
        /// <returns>True if parse successful, otherwise false.</returns>
        public virtual bool TryParseOAuthCallbackUrl(Uri url, out FacebookOAuthResult facebookOAuthResult)
        {
            facebookOAuthResult = null;

            try
            {
                facebookOAuthResult = ParseOAuthCallbackUrl(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Parse the url to <see cref="FacebookOAuthResult"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual FacebookOAuthResult ParseOAuthCallbackUrl(Uri uri)
        {
            var parameters = new Dictionary<string, object>();

            bool found = false;
            if (!string.IsNullOrEmpty(uri.Fragment))
            {
                // #access_token and expries_in are in fragment
                var fragment = uri.Fragment.Substring(1);
                ParseUrlQueryString("?" + fragment, parameters, true);

                if (parameters.ContainsKey("access_token"))
                    found = true;
            }

            // code, state, error_reason, error and error_description are in query
            // ?error_reason=user_denied&error=access_denied&error_description=The+user+denied+your+request.
            var queryPart = new Dictionary<string, object>();
            ParseUrlQueryString("?" + uri.Query, queryPart, true);

            if (queryPart.ContainsKey("code") || (queryPart.ContainsKey("error") && queryPart.ContainsKey("error_description")))
                found = true;

            foreach (var kvp in queryPart)
                parameters[kvp.Key] = kvp.Value;

            if (found)
                return new FacebookOAuthResult(parameters);

            throw new InvalidOperationException("Could not parse Facebook OAuth url.");
        }
    }
}