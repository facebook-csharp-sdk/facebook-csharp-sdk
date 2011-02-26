// --------------------------------
// <copyright file="FacebookApp.cs" company="Facebook C# SDK">
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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json.Linq;
    using Facebook.Web;
    using System.ComponentModel;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    [Obsolete]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignTimeVisible(false)]
    [Browsable(false)]
    public class FacebookApp : FacebookAppBase
    {
        private FacebookWebRequest m_request;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/>
        /// class with values stored in the application configuration file
        /// or with only the default values if the configuration
        /// file does not have the values set.
        /// </summary>
        public FacebookApp()
        {
            this.m_request = new FacebookWebRequest();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/>
        /// class with values provided. Does not require configuration
        /// file to be set.
        /// </summary>
        /// <param name="settings">The facebook settings for the application.</param>
        public FacebookApp(IFacebookApplication settings)
        {
            Contract.Requires(settings != null);

            this.m_request = new FacebookWebRequest(settings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/>
        /// class with only an access_token set. From this state
        /// sessions will not be accessable.
        /// </summary>
        /// <param name="accessToken">The Facebook access token.</param>
        public FacebookApp(string accessToken)
        {
            Contract.Requires(!String.IsNullOrEmpty(accessToken));

            this.Session = new FacebookSession(accessToken);
        }

        /// <summary>
        /// Gets or sets the maximum number of times to retry an api
        /// call after experiencing a recoverable exception.
        /// </summary>
        /// <value>The max retries.</value>
        public int MaxRetries
        {
            get
            {
                return this.FacebookClient.MaxRetries;
            }
            set
            {
                Contract.Requires(value >= 0);
                this.FacebookClient.MaxRetries = value;
            }
        }

        /// <summary>
        /// Gets or sets the value in seconds to wait before retrying, with exponential roll off.
        /// </summary>
        /// <value>The retry delay.</value>
        public int RetryDelay
        {
            get
            {
                return this.FacebookClient.RetryDelay;
            }
            set
            {
                Contract.Requires(value >= 0);
                this.FacebookClient.RetryDelay = value;
            }
        }

        /// <summary>
        /// Gets the signed request.
        /// </summary>
        /// <value>The signed request.</value>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                return this.m_request.SignedRequest;
            }
        }

        /// <summary>
        /// Gets or sets the active user session.
        /// </summary>
        /// <value>The session.</value>
        public override FacebookSession Session
        {
            get
            {
                return this.m_request.Session;
            }
        }


        /// <summary>
        /// <para>Get a Login URL for use with redirects. By default, full page redirect is
        /// assumed. If you are using the generated URL with a window.open() call in
        /// JavaScript, you can pass in display=popup as part of the parameters.</para>
        /// <para>The parameters:</para>
        /// <para>   - next: the url to go to after a successful login</para>
        /// <para>   - cancel_url: the url to go to after the user cancels</para>
        /// <para>   - req_perms: comma separated list of requested extended perms</para>
        /// <para>   - display: can be "page" (default, full page) or "popup"</para>
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para>Get a Logout URL suitable for use with redirects.</para>
        /// <para>The parameters:</para>
        /// <para>   - next: the url to go to after a successful logout</para>
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLogoutUrl(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para>Get a Logout URL suitable for use with redirects.</para>
        /// <para>The parameters:</para>
        /// <para>    - next: the url to go to after a successful logout</para>
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginStatusUrl(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(this.m_request != null);
        }
    }
}