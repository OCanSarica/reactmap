namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "attribute")]
    public class Attribute : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "name")]
        public string Name { get; set; }
    }
}