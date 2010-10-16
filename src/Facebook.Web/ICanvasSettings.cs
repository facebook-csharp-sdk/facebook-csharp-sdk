using System;
using System.Diagnostics.Contracts;

namespace Facebook.Web
{
    /// <summary>
    /// Represents the Facebook application's canvas settings.
    /// </summary>
    [ContractClass(typeof(CanvasSettingsContracts))]
    public interface ICanvasSettings
    {
        /// <summary>
        /// The base url of your application on Facebook.
        /// </summary>
        Uri CanvasPageUrl { get; set; }

        /// <summary>
        /// Facebook pulls the content for your application's 
        /// canvas pages from this base url.
        /// </summary>
        Uri CanvasUrl { get; set; }

        /// <summary>
        /// The url to return the user after they
        /// cancel authorization.
        /// </summary>
        Uri AuthorizeCancelUrl { get; set; }
    }

#pragma warning disable 1591
    [ContractClassFor(typeof(ICanvasSettings))]
    internal abstract class CanvasSettingsContracts : ICanvasSettings
    {
        public Uri CanvasPageUrl
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return default(Uri);
            }
            set
            {
                
            }
        }

        public Uri CanvasUrl
        {
            get
            {
                return default(Uri);
            }
            set
            {
            }
        }

        public Uri AuthorizeCancelUrl
        {
            get
            {
                return default(Uri);
            }
            set
            {

            }
        }
    }
#pragma warning restore 1591
}