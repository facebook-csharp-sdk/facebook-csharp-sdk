// --------------------------------
// <copyright file="GraphResultCollection.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Facebook.Api.Graph {
    public class GraphResultCollection : Collection<dynamic> {

        /// <summary>
        /// The query parameters for the next
        /// </summary>
        public string Next { get; set; }
        public string Previous { get; set; } 

    }
}
