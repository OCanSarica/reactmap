namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "table_attribute_property")]
    public class TableAttributeProperty : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "table_attribute_id")]
        public int TableAttributeId { get; set; }

        [core.Attributes.Column(Name = "attribute_property_id")]
        public int AttributePropteryId { get; set; }

        [core.Attributes.Column(Name = "value")]
        public string Value { get; set; }
    }
}