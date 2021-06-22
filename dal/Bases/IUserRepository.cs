using core.Bases;
using dal.Entities;
using dal.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace dal.Bases
{
    public interface IUserRepository : IRepository<User>
    {
        List<UserRolePermissionDto> GetWithRolesAndPermissions(long _userId);
    }
}
