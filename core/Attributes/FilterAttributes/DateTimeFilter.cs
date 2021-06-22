using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace core.Attributes.FilterAttributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DateTimeFilterAttribute : FilterBaseAttribute
    {
        public DateTimeFilterAttribute()
        {

        }
        public override void Prepare(PropertyInfo property, String columnName)
        {
            base.Prepare(property, columnName);

            var filter = FilterBase as DateTimeFilter;
            filter.Plugin = "datetimepicker";
        }

        protected override FilterBase CreateFilter()
        {
            return new DateTimeFilter();
        }

    }

    [DataContract]
    public class DateTimeFilter : FilterBase
    {
        [DataMember(Name = "datetimepicker")]
        public string Plugin { get; set; }
    }
}