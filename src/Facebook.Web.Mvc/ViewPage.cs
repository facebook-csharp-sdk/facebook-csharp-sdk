// --------------------------------
// <copyright file="ViewPage.cs" company="Thuzi, LLC">
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
    public class ViewPage : System.Web.Mvc.ViewPage {

        private XfbmlHelper xfbml;

        public XfbmlHelper Xfbml {
            get {
                if (xfbml == null) {
                    xfbml = new XfbmlHelper();
                }
                return this.xfbml;
            }
        }
    
    }

    public class ViewPage<TModel> : System.Web.Mvc.ViewPage<TModel> {

        private XfbmlHelper xfbml;

        public XfbmlHelper Xfbml {
            get {
                if (xfbml == null) {
                    xfbml = new XfbmlHelper();
                }
                return this.xfbml;
            }
        }
    
    }
}
