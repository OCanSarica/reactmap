using dal.Bases;
using dal.Entities;
using dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace bll.Bases
{
    public interface IUserService : IService
    {
        ResponseValidateUserDto ValidateUser(RequestValidateUserDto _dto);
    }
}
