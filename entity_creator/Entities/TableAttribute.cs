namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "table_attribute")]
    public class TableAttribute : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id")]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "table_id", Identity = true)]
        public int TableId { get; set; }

        [core.Attributes.Column(Name = "attribute_id")]
        public int AttributeId { get; set; }
    }
}