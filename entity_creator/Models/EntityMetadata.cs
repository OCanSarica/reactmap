using System.Collections.Generic;

namespace entity_creator.Models
{
    public class EntityMetadata
    {
        public List<EntityAttributeMetadata> AttributeList { get; set; }

        public List<EntityPropertyMetadata> PropertyList { get; set; }
        
        public string EntityName { get; set; }

        public string AssemblyName { get; set; }

        public bool IsView { get; set; }

        public EntityMetadata()
        {
            PropertyList = new List<EntityPropertyMetadata>();

            AttributeList = new List<EntityAttributeMetadata>();
        }
    }
}