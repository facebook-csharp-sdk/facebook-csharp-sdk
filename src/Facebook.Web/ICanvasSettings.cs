using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Facebook.Web
{
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

  
    [ContractClassFor(typeof(ICanvasSettings))]
    public abstract class CanvasSettingsContracts : ICanvasSettings
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

}