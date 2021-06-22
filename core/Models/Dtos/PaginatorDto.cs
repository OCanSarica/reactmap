using System;

namespace core.Models
{
    public class PaginatorDto
    {
        public string[] OrderValues { get; set; }

        public string[] OrderColumns { get; set; }

        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public string[] FilterColumns { get; set; }

        public string[] FilterValues { get; set; }

        public string[] SearchColumns { get; set; }

        public string SearchText { get; set; }
    }
}
