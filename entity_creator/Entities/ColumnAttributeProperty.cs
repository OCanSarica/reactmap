namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "column_attribute_property")]
    public class ColumnAttributeProperty : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "column_attribute_id")]
        public int ColumnAttributeId { get; set; }

        [core.Attributes.Column(Name = "attribute_property_id")]
        public int AttributePropteryId { get; set; }

        [core.Attributes.Column(Name = "value")]
        public string Value { get; set; }
    }
}
