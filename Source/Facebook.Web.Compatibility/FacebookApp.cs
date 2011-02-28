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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides access to the Facebook Platform.
    /// </summary>
    [Obsolete("Use FacebookWebClient instead.")]
    [TypeForwardedFrom("Facebook, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public class FacebookApp : FacebookAppBase
    {

        /// <summary>
        /// The collection of Facebook error types that should be retried.
        /// </summary>
        private static Collection<string> retryErrorTypes = new Collection<string>()
        {
            "OAuthException", // Graph OAuth Exception
            "190", // Rest OAuth Exception
            "Unknown", // No error info returned by facebook.
        };

        /// <summary>
        /// How many times to retry a command if an error occurs until we give up.
        /// </summary>
        private int maxRetries = 2;

        /// <summary>
        /// How long in milliseconds to wait before retrying.
        /// </summary>
        private int retryDelay = 500;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/>
        /// class with values stored in the application configuration file
        /// or with only the default values if the configuration
        /// file does not have the values set.
        /// </summary>
        public FacebookApp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApp"/>
        /// class with values provided. Does not require configuration
        /// file to be set.
        /// </summary>
        /// <param name="settings">The facebook settings for the application.</param>
        public FacebookApp(IFacebookSettings settings)
        {
            Contract.Requires(settings != null);
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
        /// Gets a collection of Facebook error types that
        /// should be retried in the event of a failure.
        /// </summary>
        protected virtual Collection<string> RetryErrorTypes
        {
            get { return retryErrorTypes; }
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
                return this.maxRetries;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.maxRetries = value;
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
                return this.retryDelay;
            }
            set
            {
                Contract.Requires(value >= 0);

                this.retryDelay = value;
            }
        }

#if !SILVERLIGHT && !CLIENTPROFILE
        /// <summary>
        /// Gets the signed request.
        /// </summary>
        /// <value>The signed request.</value>
        public FacebookSignedRequest SignedRequest
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
#endif


#if CLIENTPROFILE || SILVERLIGHT

        /// <summary>
        /// <para>Get a Login URL for use with redirects. By default,  a popup redirect is
        /// assumed.</para>
        /// <para>The parameters:</para>
        /// <para>   - display: can be "page" (default, full page) or "popup"</para>
        /// </summary>
        /// <param name="parameters">Custom url parameters.</param>
        /// <returns>The URL for the login flow.</returns>
        public override Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

#else

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

#endif

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
        /// This method invokes the supplied delegate with retry logic wrapped around it.  No values are returned.  If the delegate raises
        /// recoverable Facebook server or client errors, then the supplied delegate is reinvoked after a certain amount of delay
        /// until the retry limit is exceeded, at which point the exception is rethrown. Other exceptions are not caught and will
        /// be visible to callers.
        /// </summary>
        /// <param name="body">The delegate to invoke within the retry code.</param>
        protected void WithMirrorRetry(Action body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    body();
                    return;
                }
                catch (FacebookApiException ex)
                {
                    if (!this.RetryErrorTypes.Contains(ex.ErrorType))
                    {
                        throw;
                    }
                    else
                    {
                        if (retryCount >= this.maxRetries)
                        {
                            throw;
                        }
                    }
                }
                catch (WebException)
                {
                    if (retryCount >= this.maxRetries)
                    {
                        throw;
                    }
                }

                // Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(this.retryDelay);
                retryCount += 1;
            }
        }

        /// <summary>
        /// This method invokes the supplied delegate with retry logic wrapped around it and returns the value of the delegate.
        /// If the delegate raises recoverable Facebook server or client errors, then the supplied delegate is reinvoked after
        /// a certain amount of delay until the retry limit is exceeded, at which point the exception is rethrown. Other
        /// exceptions are not caught and will be visible to callers.
        /// </summary>
        /// <typeparam name="TReturn">The type of object being returned</typeparam>
        /// <param name="body">The delegate to invoke within the retry logic which will produce the value to return</param>
        /// <returns>The value the delegate returns</returns>
        protected TReturn WithMirrorRetry<TReturn>(Func<TReturn> body)
        {
            Contract.Requires(body != null);

            int retryCount = 0;

            while (true)
            {
                try
                {
                    return body();
                }
                catch (FacebookApiException ex)
                {
                    if (!this.RetryErrorTypes.Contains(ex.ErrorType))
                    {
                        throw;
                    }
                    else
                    {
                        if (retryCount >= this.maxRetries)
                        {
                            throw;
                        }
                    }
                }
                catch (WebException)
                {
                    if (retryCount >= this.maxRetries)
                    {
                        throw;
                    }
                }

                // Sleep for the retry delay before we retry again
                System.Threading.Thread.Sleep(this.retryDelay);
                retryCount += 1;
            }
        }

        /// <summary>
        /// The code contracts invarient object method.
        /// </summary>
        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code contracts invarient method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Code contracts invarient method.")]
        private void InvarientObject()
        {
            Contract.Invariant(this.maxRetries >= 0);
            Contract.Invariant(this.retryDelay >= 0);
            Contract.Invariant(retryErrorTypes != null);
        }
    }
}