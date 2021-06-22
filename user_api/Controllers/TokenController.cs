using System;
using System.Collections.Generic;
using System.Linq;
using dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using core.Models;
using core.Tools;
using bll.Bases;
using dto.Models;
using bll.Extensions;
using dto.Enums;
using core.Filters;

namespace user_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _UserService;

        private readonly ILogService _LogService;

        private readonly IConfiguration _Configuration;

        public TokenController(
            IUserService _userService, 
            ILogService _logService,
            IConfiguration _configuration)
        {
            _UserService = _userService;

            _LogService = _logService;

            _Configuration = _configuration;
        }

        [HttpPost]
        public Response GetToken(RequestGetTokenDto _dto)
        {
            var _result = new Response();

            try
            {
                var _responseValidateUser = _UserService.ValidateUser(_dto.ConverToDto());

                if (!_responseValidateUser.Success)
                {
                    _result.Exception = _responseValidateUser.Exception;

                    return _result;
                }

                var _token = TokenTool.Instance.GenerateToken(
                    new TokenUser
                    {
                        Permissions = _responseValidateUser.Permissions.
                            Select(x => x.Name).
                            ToList(),
                        Roles = _responseValidateUser.Roles.
                            Select(x => x.Name).
                            ToList(),
                        UserId = _responseValidateUser.User.Id
                    });

                if (string.IsNullOrEmpty(_token))
                {
                    _result.Exception = "Token oluşturulamadı.";

                    return _result;
                }

                _LogService.LogUser(new RequestLogUserDto
                {
                    UserLogType = UserLogType.LogIn,
                    UserId = _responseValidateUser.User.Id
                });

                LogTool.Instance.Debug($"Giriş yapıldı => {_dto.Username}");

                _result.Data = _responseValidateUser.ConverToDto(_token);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }
    }
}
