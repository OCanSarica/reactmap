namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "column_attribute")]
    public class ColumnAttribute : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id")]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "column_id", Identity = true)]
        public int ColumnId { get; set; }

        [core.Attributes.Column(Name = "attribute_id")]
        public int AttributeId { get; set; }
    }
}