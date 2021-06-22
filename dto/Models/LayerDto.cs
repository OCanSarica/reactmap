using System;
using System.Collections.Generic;
using System.Text;

namespace dto.Models
{
    public class LayerDto
    {
        public long LayerId { get; set; }

        public string Label { get; set; }

        public long ParentLayerId { get; set; }

        public string CqlFilter { get; set; }

        public bool Checked { get; set; }

        public int MinVisibleLevel { get; set; }

        public int MaxVisibleLevel { get; set; }

        public int Opacity { get; set; }
    }
}
