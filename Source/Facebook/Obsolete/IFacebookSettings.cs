using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    [Obsolete("Use IFacebookApplication instead.")]
    public interface IFacebookSettings : IFacebookApplication
    {

        int MaxRetries { get; set; }
        int RetryDelay { get; set; }

    }
}
