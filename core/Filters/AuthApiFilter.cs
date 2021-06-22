using core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace core.Filters
{
    public class AuthApiFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public string Roles { get; set; }

        public string Permissions { get; set; }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext _context,
            ActionExecutionDelegate _next)
        {
            if (!_context.HttpContext.Request.Headers.TryGetValue(
                "token",
                out StringValues _token))
            {
                _context.Result = new AuthenticationFailureResult("missing token");

                return;
            }

            var _tokenValidation = TokenTool.Instance.ValidateToken(_token.First());

            if (!_tokenValidation.Validated)
            {
                _context.Result = new AuthenticationFailureResult("invalid token");

                return;
            }

            if (!string.IsNullOrEmpty(Roles))
            {
                var _result = false;

                foreach (var _actionRole in Roles.Split(','))
                {
                    if (_tokenValidation.TokenUser.Roles.Any(x => x == _actionRole))
                    {
                        _result = true;

                        break;
                    }
                }

                if (!_result)
                {
                    _context.Result = new AuthenticationFailureResult("no authority");

                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(Permissions))
            {
                var _result = false;

                foreach (var _actionPermission in Permissions.Split(','))
                {
                    if (_tokenValidation.TokenUser.Permissions.Any(x => x == _actionPermission))
                    {
                        _result = true;

                        break;
                    }
                }

                if (!_result)
                {
                    _context.Result = new AuthenticationFailureResult("no authority");

                    return;
                }
            }

            // var _actionLogAttribute = _context.ActionContext.ActionDescriptor.
            //     GetCustomAttributes<ActionLogAttribute>(false).
            //     FirstOrDefault() as ActionLogAttribute;

            // _ = LogTool.Instance.ActionLog(
            //     _context.ActionContext.ActionDescriptor.ActionName,
            //     _context.ActionContext.ControllerContext.Controller.GetType().FullName,
            //     _actionLogAttribute?.Explain,
            //     LogTool.Instance.GetIpAddress(),
            //     _tokenValidation.TokenUser.UserId);

            _context.HttpContext.Request.Headers.Add(
                "token_user_id",
                _tokenValidation.TokenUser.UserId.ToString());

            await _next();
        }
    }

    public class AuthenticationFailureResult : IActionResult
    {
        private readonly string _Phrase;

        public AuthenticationFailureResult(string _phrase)
        {
            _Phrase = _phrase;
        }

        public async Task ExecuteResultAsync(ActionContext _context)
        {
            var _result = new ObjectResult(_Phrase)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

            await _result.ExecuteResultAsync(_context);
        }
    }
}
