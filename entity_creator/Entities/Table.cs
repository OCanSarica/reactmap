namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "table_")]
    public class Table : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "table_name")]
        public string TableName { get; set; }

        [core.Attributes.Column(Name = "display_name")]
        public string DisplayName { get; set; }

        [core.Attributes.Column(Name = "serializable_column")]
        public bool SerializableColumn { get; set; }

        [core.Attributes.Column(Name = "entity_name")]
        public string EntityName { get; set; }
    }
}