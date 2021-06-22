namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "relation")]
    public class Relation
    {
        [core.Attributes.Column(Name = "relation_id", Identity = true)]
        public int RelationId { get; set; }

        [core.Attributes.Column(Name = "relation_name")]
        public string RelationName { get; set; }
    }
}