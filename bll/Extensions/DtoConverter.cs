using core.Bases;
using dal.Entities;
using dto.Models;
using System;
using core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace bll.Extensions
{
    public static class DtoConverter
    {
        public static UserLog ConvertToEntity(this RequestLogUserDto _dto) =>
            new UserLog
            {
                Date = DateTime.Now,
                TypeId = (int)_dto.UserLogType,
                UserId = _dto.UserId
            };

        public static ResponseAddEntityDto ConvertToDto(this EntityBase _entity) =>
            new ResponseAddEntityDto
            {
                Id = _entity.Id
            };

        public static ActionLog ConvertToEntity(this RequestLogActionDto _dto) =>
            new ActionLog
            {
                Action = _dto.Action,
                Controller = _dto.Controller,
                Date = DateTime.Now,
                Explanation = _dto.Explanation,
                Ip = _dto.Ip,
                UserId = _dto.UserId
            };

        public static RequestValidateUserDto ConverToDto(this RequestGetTokenDto _dto) =>
            new RequestValidateUserDto
            {
                Username = _dto.Username,
                Password = _dto.Password
            };

        public static ResponseGetTokenDto ConverToDto(
            this ResponseValidateUserDto _dto,
            string _token) =>
            new ResponseGetTokenDto
            {
                Permissions = _dto.Permissions,
                Roles = _dto.Roles,
                Token = _token,
                User = _dto.User
            };

        public static LayerDto ConverToDto(this LayerControl _entity) =>
            new LayerDto
            {
                Checked = _entity.Checked,
                CqlFilter = _entity.CqlFilter,
                LayerId = _entity.LayerId,
                MaxVisibleLevel = _entity.MaxVisibleLevel,
                MinVisibleLevel = _entity.MinVisibleLevel,
                Opacity = _entity.Opacity,
                ParentLayerId = _entity.ParentLayerId,
                Label = _entity.LayerLabel
            };

        public static Poi ConvertToEntity(this PoiDto _dto) =>
             new Poi
             {
                 Geoloc = _dto.Wkt?.WktToGeometry(),
                 Id = _dto.Id,
                 Name = _dto.Name,
             };

        public static PoiDto ConvertToDto(this Poi _entity) =>
            new PoiDto
            {
                Wkt = _entity.Geoloc.ToWKt(),
                Id = _entity.Id,
                Name = _entity.Name,
            };
    }
}
