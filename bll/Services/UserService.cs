using bll.Bases;
using core.Models;
using core.Tools;
using dal.Bases;
using dal.Entities;
using dto.Enums;
using dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace bll.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository _UserRepository;

        private readonly ILogService _LogService;

        public UserService(
            IUnitOfWork _unitofWork,
            ILogService _logService) : base(_unitofWork)
        {
            _UserRepository = _unitofWork.GetUserRepository();

            _LogService = _logService;
        }

        public ResponseValidateUserDto ValidateUser(RequestValidateUserDto _dto)
        {
            var _result = new ResponseValidateUserDto();

            var _user = _UserRepository.Get(x =>
                    x.Username == _dto.Username &&
                    !x.IsDeleted &&
                    x.Status);

            if (_user == null)
            {
                _LogService.LogUser(new RequestLogUserDto
                {
                    UserLogType = UserLogType.WrongLogIn
                });

                _result.Exception = "Kullanıcı adı veya şifre yanlış!";

                return _result;
            }

            if (EncryptionTool.Instance.Decrypt(_user.Password) != _dto.Password)
            {
                _LogService.LogUser(new RequestLogUserDto
                {
                    UserId = _user.Id,
                    UserLogType = UserLogType.WrongLogIn
                });

                _result.Exception = "Kullanıcı adı veya şifre yanlış!";

                return _result;
            }

            var _userRolePermssion = _UserRepository.GetWithRolesAndPermissions(_user.Id);

            var _roles = _userRolePermssion.
                Select(x => x.Role).
                Distinct().
                ToList();

            if (_roles.Count == 0)
            {
                _result.Exception = "Rol veya işlev bulunamadı.";

                return _result;
            }

            var _permissions = _userRolePermssion.
                Select(x => x.Permission).
                Distinct().
                ToList();

            if (_permissions.Count == 0)
            {
                _result.Exception = "Rol veya işlev bulunamadı.";

                return _result;
            }

            _result.Permissions = _permissions;

            _result.Roles = _roles;

            _result.Success = true;

            _result.User = _user;

            return _result;
        }

        public void Save() => _UnitOfWork.Save();
    }
}
