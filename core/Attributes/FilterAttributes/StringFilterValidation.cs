using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [DataContract]
    public class StringFilterValidation
    {
        [DataMember(Name = "max")]
        public int Max { get; set; }

        [DataMember(Name = "min")]
        public int Min { get; set; }

        [DataMember(Name = "fomat")]
        public string Format { get; set; }
    }
}