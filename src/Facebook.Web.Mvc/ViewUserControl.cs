// --------------------------------
// <copyright file="ViewUserControl.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebookgraphtoolkit.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebook.Web.Mvc {
    public class ViewUserControl : System.Web.Mvc.ViewUserControl {
        
        private XfbmlHelper xfbml;

        public XfbmlHelper Xfbml {
            get {
                if (xfbml == null) {
                    xfbml = new XfbmlHelper();
                }
                return xfbml;
            }
        }

    }

    public class ViewUserControl<TModel> : System.Web.Mvc.ViewUserControl<TModel> {
        
        private XfbmlHelper xfbml;

        public XfbmlHelper Xfbml {
            get {
                if (xfbml == null) {
                    xfbml = new XfbmlHelper();
                }
                return xfbml;
            }
        }
    }
}
