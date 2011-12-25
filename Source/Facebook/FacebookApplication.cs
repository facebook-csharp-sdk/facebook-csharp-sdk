// --------------------------------
// <copyright file="FacebookApplication.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace Facebook
{
    using System;

    /// <summary>
    /// Represents the Facebook Context class.
    /// </summary>
    public class FacebookApplication
    {
        /// <summary>
        /// Current Facebook application.
        /// </summary>
        private static readonly FacebookApplication Instance = new FacebookApplication();

        /// <summary>
        /// The current Facebook application.
        /// </summary>
        private IFacebookApplication _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApplication"/> class. 
        /// </summary>
        public FacebookApplication()
        {
#if !(SILVERLIGHT || WINRT)
            _current = FacebookConfigurationSection.Current;
#endif
        }

        /// <summary>
        /// Gets the current Facebook application.
        /// </summary>
        public static IFacebookApplication Current
        {
            get { return Instance.InnerCurrent; }
        }

        /// <summary>
        /// Set the current Facebook application.
        /// </summary>
        /// <param name="facebookApplication">
        /// The Facebook application.
        /// </param>
        public static void SetApplication(IFacebookApplication facebookApplication)
        {
            Instance.InnerSetApplication(facebookApplication);
        }

        /// <summary>
        /// Set the current Facebook application.
        /// </summary>
        /// <param name="getFacebookApplication">
        /// The get Facebook application.
        /// </param>
        public static void SetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            Instance.InnerSetApplication(getFacebookApplication);
        }

        /// <summary>
        /// Gets InnerCurrent.
        /// </summary>
        public IFacebookApplication InnerCurrent
        {
            get { return _current ?? new DefaultFacebookApplication(); }
        }

        /// <summary>
        /// Set the inner application.
        /// </summary>
        /// <param name="facebookApplication">
        /// The Facebook application.
        /// </param>
        public void InnerSetApplication(IFacebookApplication facebookApplication)
        {
            if (facebookApplication == null)
                throw new ArgumentNullException("facebookApplication");

            _current = facebookApplication;
        }

        /// <summary>
        /// Set the inner application.
        /// </summary>
        /// <param name="getFacebookApplication">
        /// The get Facebook application.
        /// </param>
        public void InnerSetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            if (getFacebookApplication == null)
                throw new ArgumentNullException("getFacebookApplication");

            _current = getFacebookApplication();
        }
    }
}