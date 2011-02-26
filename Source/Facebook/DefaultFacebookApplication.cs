using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    public class DefaultFacebookApplication : IFacebookApplication
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string SiteUrl { get; set; }

        public string CanvasPage { get; set; }

        public string CanvasUrl { get; set; }

        public string CancelUrlPath { get; set; }
    }
}
