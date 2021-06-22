using System;
using Microsoft.AspNetCore.Mvc;
using core.Filters;
using core.Tools;
using core.Models;
using bll.Bases;
using core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace map_api.Controllers
{
    [ApiController]
    [AuthApiFilter]
    [Route("api/[controller]")]
    public class LayerController : ControllerBase
    {
        private readonly ILayerService _ILayerService;

        public LayerController(ILayerService _layerService)
        {
            _ILayerService = _layerService;
        }

        [HttpGet]
        public Response GetLayers()
        {
            var _result = new Response();

            try
            {
                var _userId = HttpContext.GetUserId();

                _result.Data = _ILayerService.GetLayerControl(_userId);

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