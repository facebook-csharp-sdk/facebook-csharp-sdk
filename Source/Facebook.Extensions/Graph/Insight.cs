using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Facebook.Graph
{
    [DataContract]
    public class Insight
    {

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "period")]
        public string Period { get; set; }

        [DataMember(Name = "values")]
        public ICollection<InsightValue> Values { get; set; }

    }

    [DataContract]
    public class InsightValue
    {
        [DataMember(Name = "value")]
        public float Value { get; set; }

        [DataMember(Name = "end_time")]
        public DateTime EndTime { get; set; }
    }

    [DataContract]
    public class InsightCollectionItem
    {
        [DataMember(Name = "data")]
        public List<Insight> Data { get; set; }

        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }
    }

    [DataContract]
    public class Paging
    {
        [DataMember(Name = "next")]
        public string Next { get; set; }
        [DataMember(Name = "previous")]
        public string Previous { get; set; }
    }

}
