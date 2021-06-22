using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [DataContract]
    public class NumberFilterValidation
    {
        [DataMember(Name = "max")]
        public int Max { get; set; }

        [DataMember(Name = "min")]
        public int Min { get; set; }

        [DataMember(Name = "step")]
        public double Step { get; set; }
    }
}