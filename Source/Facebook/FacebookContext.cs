// --------------------------------
// <copyright file="FacebookContext.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the Facebook Context class.
    /// </summary>
    public class FacebookContext
    {
        /// <summary>
        /// Current Facebook application.
        /// </summary>
        private static readonly FacebookContext Instance = new FacebookContext();

        /// <summary>
        /// Gets the current Facebook application.
        /// </summary>
        public static IFacebookApplication Current
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookApplication>() != null);
                return Instance.InnerCurrent;
            }
        }

        /// <summary>
        /// Set the current facebook application.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public static void SetApplication(IFacebookApplication facebookApplication)
        {
            Contract.Requires(facebookApplication != null);
            Instance.InnerSetApplication(facebookApplication);
        }

        /// <summary>
        /// Set the current facebook application.
        /// </summary>
        /// <param name="getFacebookApplication">
        /// The get facebook application.
        /// </param>
        public static void SetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            Contract.Requires(getFacebookApplication != null);

            Instance.InnerSetApplication(getFacebookApplication);
        }

#if !SILVERLIGHT
        /// <summary>
        /// The current facebook application.
        /// </summary>
        private IFacebookApplication current = FacebookConfigurationSection.Current;
#else
        /// <summary>
        /// The current facebook application.
        /// </summary>
        private IFacebookApplication current = new NullFacebookApplication();
#endif

        /// <summary>
        /// Gets InnerCurrent.
        /// </summary>
        public IFacebookApplication InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<IFacebookApplication>() != null);
                return this.current ?? new NullFacebookApplication();
            }
        }

        /// <summary>
        /// Set the inner application.
        /// </summary>
        /// <param name="facebookApplication">
        /// The facebook application.
        /// </param>
        public void InnerSetApplication(IFacebookApplication facebookApplication)
        {
            Contract.Requires(facebookApplication != null);

            this.current = facebookApplication;
        }

        /// <summary>
        /// Set the inner application.
        /// </summary>
        /// <param name="getFacebookApplication">
        /// The get facebook application.
        /// </param>
        public void InnerSetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            Contract.Requires(getFacebookApplication != null);

            this.current = getFacebookApplication();
        }

        /// <summary>
        /// Represents a null Facebook application.
        /// </summary>
        private class NullFacebookApplication : IFacebookApplication
        {
            /// <summary>
            /// Gets the application id.
            /// </summary>
            public string AppId
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the application secret.
            /// </summary>
            public string AppSecret
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the site url.
            /// </summary>
            public string SiteUrl
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the canvas page.
            /// </summary>
            public string CanvasPage
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the canvas url.
            /// </summary>
            public string CanvasUrl
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the url to return the user after they cancel authorization.
            /// </summary>
            public string CancelUrlPath
            {
                get { return null; }
            }
        }
    }
}