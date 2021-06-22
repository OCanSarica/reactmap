

namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "table_column")]
    public class TableColumn : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "table_id")]
        public int TableId { get; set; }

        [core.Attributes.Column(Name = "column_name")]
        public string ColumnName { get; set; }

        [core.Attributes.Column(Name = "display_name")]
        public string DisplayName { get; set; }

        [core.Attributes.Column(Name = "serializable_column")]
        public bool SerializableColumn { get; set; }

        [core.Attributes.Column(Name = "identity_column")]
        public bool IdentityColumn { get; set; }

        [core.Attributes.Column(Name = "sequence_name")]
        public string SequenceName { get; set; }

        [core.Attributes.Column(Name = "property_name")]
        public string PropertyName { get; set; }

        [core.Attributes.Column(Name = "order_by")]
        public int OrderBy { get; set; }
    }
}