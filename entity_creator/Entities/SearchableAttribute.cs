namespace entity_creator.Entities
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SearchableAttribute : System.Attribute
    {
        public string Name { get; set; }
    }
}