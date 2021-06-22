using System;
using System.Collections.Generic;

namespace entity_creator.Models
{
    public class EntityAttributeMetadata
    {
        public Type AttributeType { get; set; }

        public List<string> PropertyNames { get; set; }

        public List<object> PropertyValues { get; set; }

        public List<object> ConstructorValues { get; set; }

        public List<Type> ConstructorTypes { get; set; }

        public EntityAttributeMetadata()
        {
            PropertyNames = new List<string>();
            PropertyValues = new List<object>();
            ConstructorValues = new List<object>();
            ConstructorTypes = new List<Type>();
            AttributeType = null;
        }
    }
}