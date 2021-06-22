using dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dal.Models.Dtos
{
    public class UserRolePermissionDto
    {
        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}
