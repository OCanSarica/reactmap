using System;
using System.Collections.Generic;

namespace core.Models
{
    public class PaginatorResponseDto<T>
    {
        public List<T> Rows { get; set; }

        public long Count { get; set; }
    }
}
