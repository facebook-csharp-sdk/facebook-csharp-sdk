using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web
{
    public interface ICanvasSettings
    {
        /// <summary>
        /// The url to your canvas application.
        /// </summary>
        Uri CanvasPageUrl { get; set; }

        /// <summary>
        /// The url to return the user after they
        /// cancel authorization.
        /// </summary>
        Uri AuthorizeCancelUrl { get; set; }
    }
}
