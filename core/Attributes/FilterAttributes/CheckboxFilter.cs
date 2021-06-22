using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Attributes.FilterAttributes
{
    /// <summary>
    /// Eğer Values verilirse LookUpTable passif olur
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CheckboxFilterAttribute : Attribute
    {

    }


    [DataContract]
    public class CheckboxFilter : FilterBase
    {
        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "values")]
        public Dictionary<object, object> Values { get; set; }
    }
}
