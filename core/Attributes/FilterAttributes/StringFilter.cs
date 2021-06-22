    using System;
    using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StringFilterAttribute : FilterBaseAttribute
    {
        public string Placeholder { get; set; }

        public int Max { get; set; }

        public int Min { get; set; }

        public string Format { get; set; }

        public string ReadFromFile { get; set; }

        public string ValueSeparator { get; set; }

        public string Input { get; set; }

        public StringFilterAttribute()
        {
            Min = 0;
            Max = int.MaxValue;
        }

        protected override FilterBase CreateFilter()
        {
            return new StringFilter();
        }

        public override void Prepare(System.Reflection.PropertyInfo property, String columName)
        {
            base.Prepare(property, columName);

            var selectMenuFilter = FilterBase as StringFilter;

            selectMenuFilter.Placeholder = Placeholder;

            selectMenuFilter.Validation = new StringFilterValidation();
            selectMenuFilter.Validation.Max = Max;
            selectMenuFilter.Validation.Min = Min;

            selectMenuFilter.Input = Input;
            selectMenuFilter.Validation.Format = Format;
            selectMenuFilter.ReadFromFile = ReadFromFile;
            selectMenuFilter.ValueSeparator = ValueSeparator;
        }
    }

    [DataContract]
    public class StringFilter : FilterBase
    {
        [DataMember(Name = "placeholder")]
        public string Placeholder { get; set; }

        [DataMember(Name = "validation")]
        public StringFilterValidation Validation { get; set; }

        [DataMember(Name = "readFromFile")]
        public string ReadFromFile { get; set; }

        [DataMember(Name = "value_separator")]
        public string ValueSeparator { get; set; }

        [DataMember(Name = "input")]
        public string Input { get; set; }
    }
}