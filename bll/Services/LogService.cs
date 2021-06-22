using bll.Bases;
using bll.Extensions;
using dal.Bases;
using dal.Entities;
using dto.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bll.Services
{
    public class LogService : ServiceBase, ILogService
    {
        private readonly IRepository<UserLog> _UserLogRepository;

        private readonly IRepository<ActionLog> _ActionLogRepository;

        public LogService(IUnitOfWork _unitofWork) : base(_unitofWork)
        {
            _ActionLogRepository = _unitofWork.GetRepository<ActionLog>();

            _UserLogRepository = _unitofWork.GetRepository<UserLog>();
        }

        public ResponseAddEntityDto LogUser(RequestLogUserDto _dto) =>
             _UserLogRepository.Add(_dto.ConvertToEntity()).ConvertToDto();

        public async Task<ResponseAddEntityDto> LogActionAsync(RequestLogActionDto _dto)
        {
            var _result = await _ActionLogRepository.AddAsync(_dto.ConvertToEntity());

            return _result.ConvertToDto();
        }

        public void Save() => _UnitOfWork.Save();
    }
}
