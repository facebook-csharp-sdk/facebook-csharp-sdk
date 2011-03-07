using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook
{
    internal class WebClientStateContainer
    {
        public object UserState { get; set; }
        public HttpMethod Method { get; set; }
        public Uri RequestUri { get; set; }
    }
}
