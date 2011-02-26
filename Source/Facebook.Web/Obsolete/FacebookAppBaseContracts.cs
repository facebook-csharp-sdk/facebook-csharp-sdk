// --------------------------------
// <copyright file="FacebookAppBaseContracts.cs" company="Facebook C# SDK">
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
    using System.ComponentModel;

#pragma warning disable 1591

    /// <summary>
    /// Represents the inheritable contracts for the <see cref="FacebookAppBase"/> class.
    /// </summary>
    [Obsolete]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignTimeVisible(false)]
    [Browsable(false)]
    [ContractClassFor(typeof(FacebookAppBase))]
    internal abstract class FacebookAppBaseContracts : FacebookAppBase
    {
        /// <summary>
        /// Get a Login URL for use with redirects. By default, full page redirect is
        /// assumed. If you are using the generated URL with a window.open() call in
        /// JavaScript, you can pass in display=popup as part of the parameters.
        /// The parameters:
        /// - next: the url to go to after a successful login
        /// - cancel_url: the url to go to after the user cancels
        /// - req_perms: comma separated list of requested extended perms
        /// - display: can be "page" (default, full page) or "popup"
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        /// <summary>
        /// Get a Logout URL suitable for use with redirects.
        /// The parameters:
        /// - next: the url to go to after a successful logout
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        /// <summary>
        /// Get a login status URL to fetch the status from facebook.
        /// The parameters:
        /// - ok_session: the URL to go to if a session is found
        /// - no_session: the URL to go to if the user is not connected
        /// - no_user: the URL to go to if the user is not signed into facebook
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the logout flow</returns>
        public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }
    }
#pragma warning restore 1591
}
