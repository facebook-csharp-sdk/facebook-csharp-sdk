using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook.Graph
{
    [DataContract]
    public class EducationHistoryItem
    {
        [DataMember(Name = "school")]
        public string School { get; set; }
        [DataMember(Name = "year")]
        public string Year { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
