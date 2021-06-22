using dal.Bases;
using dal.Entities;
using dto.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bll.Bases
{
    public interface ILogService : IService
    {
        ResponseAddEntityDto LogUser(RequestLogUserDto _dto);

        Task<ResponseAddEntityDto> LogActionAsync(RequestLogActionDto _dto);
    }
}
