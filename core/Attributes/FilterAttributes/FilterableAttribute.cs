using System;

namespace core.Attributes.FilterAttributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class FilterableAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public Type GeometryEntityType { get; set; }

        public string GeometryParentProperty { get; set; }

        public string GeometryChildProperty { get; set; }
    }
}