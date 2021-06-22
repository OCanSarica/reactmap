using System;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NumberFilterAttribute : FilterBaseAttribute
    {
        public int Max { get; set; }

        public int Min { get; set; }

        public double Step { get; set; }

        public NumberFilterAttribute()
        {
            Min = int.MinValue;
            Max = int.MaxValue;
            Step = 1;
        }

        protected override FilterBase CreateFilter()
        {
            return new NumberFilter();
        }

        public override void Prepare(System.Reflection.PropertyInfo property, String columName)
        {
            base.Prepare(property, columName);

            var selectMenuFilter = FilterBase as NumberFilter;

            selectMenuFilter.Validation = new NumberFilterValidation();
            selectMenuFilter.Validation.Max = Max;
            selectMenuFilter.Validation.Min = Min;
            selectMenuFilter.Validation.Step = Step;
        }
    }

    [DataContract]
    public class NumberFilter : FilterBase
    {
        [DataMember(Name = "validation")]
        public NumberFilterValidation Validation { get; set; }
    }
}