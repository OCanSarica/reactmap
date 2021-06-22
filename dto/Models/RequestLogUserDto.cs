using dto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dto.Models
{
    public class RequestLogUserDto
    {
        public UserLogType UserLogType { get; set; }

        public long UserId { get; set; }
    }
}
