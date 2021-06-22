using System.Collections.Generic;

namespace map_api.Models
{
    public class FeatureInfoDto
    {
        public string Id { get; set; }
        public string IdColumn { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}