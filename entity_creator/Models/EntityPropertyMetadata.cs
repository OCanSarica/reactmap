using System;
using System.Collections.Generic;

namespace entity_creator.Models
{
    public class EntityPropertyMetadata
    {
        public List<EntityAttributeMetadata> AttributeList { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public bool SpatialWkt { get; set; }

        public bool IsIdentity { get; set; }

        public EntityPropertyMetadata()
        {
            AttributeList = new List<EntityAttributeMetadata>();
            PropertyType = null;
            PropertyName = "";
        }
    }
}