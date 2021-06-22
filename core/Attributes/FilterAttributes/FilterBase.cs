using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [DataContract]
    public class FilterBase
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}