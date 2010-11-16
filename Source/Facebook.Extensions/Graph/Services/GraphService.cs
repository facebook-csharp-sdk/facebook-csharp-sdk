using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Graph
{
    public abstract class GraphService
    {

        public FacebookApp App { get; set; }

        public GraphService()
        {
            this.App = new FacebookApp();
        }

        public GraphService(FacebookApp app)
        {
            this.App = app;
        }

    }
}
