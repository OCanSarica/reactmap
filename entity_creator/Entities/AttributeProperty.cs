namespace entity_creator.Entities
{
    [core.Attributes.Table(Name = "attribute_property")]
    public class AttributeProperty : Bases.IEntity
    {
        [core.Attributes.Column(Name = "id", Identity = true)]
        public int Id { get; set; }

        [core.Attributes.Column(Name = "attribute_id")]
        public int AttributeId { get; set; }

        [core.Attributes.Column(Name = "name")]
        public string Name { get; set; }
    }
}