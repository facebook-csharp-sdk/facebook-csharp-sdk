using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook.Graph
{
    [DataContract]
    public class WorkHistoryItem
    {
        [DataMember(Name = "employer")]
        public string Employer { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "position")]
        public string Position { get; set; }
        [DataMember(Name = "start_date")]
        public string StartDate { get; set; }
        [DataMember(Name = "end_date")]
        public string EndDate { get; set; }

    }
}
