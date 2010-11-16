using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook.Graph
{
    [DataContract]
    public class User
    {
        /// <summary>
        /// Gets or sets the user's Facebook ID.
        /// Publicly available.
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// Publicly available.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
