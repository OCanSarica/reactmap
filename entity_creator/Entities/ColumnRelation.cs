namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "column_relation")]
    public class ColumnRelation
    {
        [core.Attributes.Column(Name = "first_column_id", Identity = true)]
        public int FirstColumnId { get; set; }

        [core.Attributes.Column(Name = "second_column_id")]
        public int SecondColumnId { get; set; }

        [core.Attributes.Column(Name = "relation_type")]
        public int RelationType { get; set; }
    }
}