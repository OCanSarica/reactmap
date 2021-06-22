namespace core.Attributes.FilterAttributes
{
    public class FilterableModel
    {
        public string EntityName { get; set; }

        public string DisplayName { get; set; }

        public int Id { get; set; }

        public string PrimaryProperty { get; set; }

        public string PrimaryColumn { get; set; }

        public string GeometryParentProperty { get; set; }

        public string GeometryChildColumn { get; set; }

        public FilterableModel(string displayName, string entityName, int id, string primaryProperty, string primaryColumn,
            string geometryParentProperty, string geometryChildColumn)
        {
            this.Id = id;
            this.EntityName = entityName;
            this.DisplayName = displayName;
            this.PrimaryProperty = primaryProperty;
            this.PrimaryColumn = primaryColumn;

            this.GeometryParentProperty = geometryParentProperty;
            this.GeometryChildColumn = geometryChildColumn;
        }
    }
}