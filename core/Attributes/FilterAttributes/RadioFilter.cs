using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    /// <summary>
    /// Eğer Values verilirse LookUpTable passif olur
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RadioFilterAttribute : FilterBaseAttribute
    {
        // Properties

        public string LookUpTable { get; set; }

        public string IdName { get; set; }

        public string ValueName { get; set; }

        public Dictionary<object, object> Values2 { get; set; }
        public object[] Ids { get; set; }

        public object[] Values { get; set; }

        //////////////////////////

        public RadioFilterAttribute()
        {
            IdName = "Id";
            ValueName = "Deger";
        }

        protected override FilterBase CreateFilter()
        {
            return new RadioFilter();
        }

        public override void Prepare(System.Reflection.PropertyInfo property, String columName)
        {
            base.Prepare(property, columName);

            var radioFilter = FilterBase as RadioFilter;
            radioFilter.Input = "radio";

            Dictionary<object, object> valueHash = new Dictionary<object, object>();

            if (Values == null)
            {
                FetchLookUptable(ref valueHash, LookUpTable, IdName, ValueName);
            }
            else
            {
                FetchValues(Ids, Values, ref valueHash);
            }
            Values2 = valueHash;
            radioFilter.Values = valueHash;
        }
    }

    [DataContract]
    public class RadioFilter : FilterBase
    {
        [DataMember(Name = "input")]
        public string Input { get; set; }

        [DataMember(Name = "values")]
        public Dictionary<object, object> Values { get; set; }
    }
}
