using System;
using System.Collections.Generic;
using System.Text;

namespace dto.Models
{
    public class LayerControlDto
    {
        public long key { get; set; }

        public string title { get; set; }

        public long ParentId { get; set; }

        public string CqlFilter { get; set; }

        public bool Checked { get; set; }

        public int MinVisibleLevel { get; set; }

        public int MaxVisibleLevel { get; set; }

        public int Opacity { get; set; }

        public List<LayerControlDto> children { get; set; }
    }
}
