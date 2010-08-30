// --------------------------------
// <copyright file="AuthenticationExtension.cs" company="Thuzi, LLC">
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

namespace Facebook.Web
{
    public static class AuthenticationExtension
    {
        public static bool IsLoggedIn(this FacebookAppBase app)
        {
            return app.Session != null;
        }
    }
}