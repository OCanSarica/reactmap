using dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dto.Models
{
    public class ResponseGetTokenDto
    {
        public string Token { get; set; }

        public User User { get; set; }

        public List<Permission> Permissions { get; set; }

        public List<Role> Roles { get; set; }
    }
}
