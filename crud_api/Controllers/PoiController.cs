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

namespace crud_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoiController : ControllerBase
    {
        private readonly IPoiService _PoiService;

        public PoiController(IPoiService _poiService)
        {
            _PoiService = _poiService;
        }

        [HttpGet]
        [Route("{_id}")]
        public Response Get(long _id)
        {
            var _result = new Response();

            try
            {
                _result.Data = _PoiService.Get(_id);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        [HttpPost]
        public Response Add(PoiDto _dto)
        {
            var _result = new Response();

            try
            {
                _result.Data = _PoiService.Add(_dto); 

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        [HttpPut]
        public Response Update(PoiDto _dto)
        {
            var _result = new Response();

            try
            {
                _PoiService.Update(_dto);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        [HttpDelete]
        [Route("{_id}")]
        public Response Remove(long _id)
        {
            var _result = new Response();

            try
            {
                _PoiService.Remove(_id);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        [HttpPost]
        [Route("[action]")]
        public Response Paginate(PaginatorDto _dto)
        {
            var _result = new Response();

            try
            {
                _result.Data = _PoiService.Paginate(_dto);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                LogTool.Instance.Error(_ex);
            }

            return _result;
        }

        [HttpGet]
        [Route("geometry/{_id}")]
        public Response GetGeometry(long _id)
        {
            var _result = new Response();

            try
            {
                _result.Data = _PoiService.Get(_id).Wkt;

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
