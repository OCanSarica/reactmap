using dto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dto.Models
{
    public class RequestLogActionDto
    {
        public string Action { get; set; }

        public string Controller { get; set; }

        public DateTime Date { get; set; }

        public string Explanation { get; set; }

        public string Ip { get; set; }

        public long UserId { get; set; }
    }
}
