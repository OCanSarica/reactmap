using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using core.Models;
using Microsoft.AspNetCore.Http;

namespace core.Tools
{
    public sealed class TokenTool
    {
        public static TokenTool Instance => _Instance.Value;

        private static readonly Lazy<TokenTool> _Instance =
            new Lazy<TokenTool>(() => new TokenTool());

        private readonly IConfiguration _Configuration;

        private readonly string _TokenKey;

        private TokenTool()
        {
            _Configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", true, true).
                Build();

            _TokenKey = _Configuration["TokenKey"];
        }

        public string GenerateToken(TokenUser _tokenUser)
        {
            var _result = string.Empty;

            try
            {
                var _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());

                var _guid = Guid.NewGuid().ToByteArray();

                var _key = Encoding.UTF8.GetBytes(_TokenKey);

                var _user = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(_tokenUser));

                var _data = new byte[_time.Length + _guid.Length +
                    _key.Length + _user.Length];

                Buffer.BlockCopy(_time, 0, _data, 0, _time.Length);

                Buffer.BlockCopy(_guid, 0, _data,
                    _time.Length, _guid.Length);

                Buffer.BlockCopy(_key, 0, _data,
                    _time.Length + _guid.Length,
                    _key.Length);

                Buffer.BlockCopy(_user, 0, _data,
                    _time.Length + _guid.Length + _key.Length,
                    _user.Length);

                _result = Convert.ToBase64String(_data.ToArray());
            }
            catch (Exception _ex)
            {
            }

            return _result;
        }

        public TokenValidation ValidateToken(string _token)
        {
            var _result = new TokenValidation();

            try
            {
                var _data = Convert.FromBase64String(_token);

                var _time = _data.Take(8).ToArray();

                var _when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));

                if (_when < DateTime.UtcNow.AddHours(-24))
                {
                    _result._Errors.Add(TokenValidationStatus.Expired);
                }

                //var _guid = _data.Skip(8).Take(16).ToArray();

                var _key = _data.Skip(24).Take(_TokenKey.Length).ToArray();

                if (_TokenKey != Encoding.UTF8.GetString(_key))
                {
                    _result._Errors.Add(TokenValidationStatus.WrongKey);
                }

                var _tokenUserString = Encoding.UTF8.GetString(
                    _data.Skip(24 + _TokenKey.Length).ToArray());

                var _tokenUser = JsonConvert.DeserializeObject<TokenUser>(_tokenUserString);

                if (_tokenUser == null || _tokenUser.UserId == 0 ||
                    _tokenUser.Roles == null || _tokenUser.Roles.Count == 0 ||
                    _tokenUser.Permissions == null || _tokenUser.Permissions.Count == 0)
                {
                    _result._Errors.Add(TokenValidationStatus.WrongUser);
                }

                _result.TokenUser = _tokenUser;
            }
            catch (Exception _ex)
            {
                _result._Errors.Add(TokenValidationStatus.Unexpected);
            }

            return _result;
        }
    }
}
