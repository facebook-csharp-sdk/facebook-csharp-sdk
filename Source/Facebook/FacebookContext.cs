using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Configuration;

namespace Facebook
{
    public class FacebookContext
    {

        private static FacebookContext _instance = new FacebookContext();

        public static IFacebookApplication Current
        {
            get { return _instance.InnerCurrent; }
        }

        public static void SetApplication(IFacebookApplication facebookApplication)
        {
            Contract.Requires(facebookApplication != null);

            _instance.InnerSetApplication(facebookApplication);
        }

        public static void SetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            Contract.Requires(getFacebookApplication != null);

            _instance.InnerSetApplication(getFacebookApplication);
        }

#if (SILVERLIGHT) 
        private IFacebookApplication _current = new NullFacebookApplication()
#else
        private IFacebookApplication _current = new ConfigFacebookApplication();
#endif
        public IFacebookApplication InnerCurrent
        {
            get
            {
                return _current;
            }
        }

        public void InnerSetApplication(IFacebookApplication facebookApplication)
        {
            Contract.Requires(facebookApplication != null);

            this._current = facebookApplication;
        }

        public void InnerSetApplication(Func<IFacebookApplication> getFacebookApplication)
        {
            Contract.Requires(getFacebookApplication != null);

            this._current = getFacebookApplication();
        }

#if SILVERLIGHT
        private class NullFacebookApplication : IFacebookApplication
        {
            public string AppId
            {
                get { return null; }
            }

            public string AppSecret
            {
                get { return null; }
            }

            public string SiteUrl
            {
                get { return null; }
            }

            public string CanvasPage
            {
                get { return null; }
            }

            public string CanvasUrl
            {
                get { return null; }
            }
        }
#else
        private class ConfigFacebookApplication : ConfigurationSection, IFacebookApplication
        {

            /// <summary>
            /// The Facebook settings stored in the configuration file.
            /// </summary>
            private static IFacebookApplication current;

            /// <summary>
            /// Gets the Facebook settings stored in the configuration file.
            /// </summary>
            private static IFacebookApplication Current
            {
                get
                {
                    if (current == null)
                    {
                        var settings = ConfigurationManager.GetSection("facebookSettings");
                        if (settings != null)
                        {
                            current = settings as FacebookConfigurationSection;
                        }
                    }

                    return current;
                }
            }

            public string AppId
            {
                get { return Current.AppId; }
            }

            public string AppSecret
            {
                get { return Current.AppSecret; }
            }

            public string SiteUrl
            {
                get { return Current.SiteUrl; }
            }

            public string CanvasPage
            {
                get { return Current.CanvasPage; }
            }

            public string CanvasUrl
            {
                get { return Current.CanvasUrl; }
            }
        }
#endif

    }

    public interface IFacebookApplication
    {
        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        string AppSecret { get; }

        /// <summary>
        /// Gets or sets the site url.
        /// </summary>
        string SiteUrl { get; }

        /// <summary>
        /// Gets or sets the canvas page.
        /// </summary>
        string CanvasPage { get; }

        /// <summary>
        /// Gets or sets the canvas url.
        /// </summary>
        string CanvasUrl { get; }
    }



}
