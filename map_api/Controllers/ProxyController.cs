using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using dal.Entities;
using dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using core.Models;
using core.Tools;
using core.Bases;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using map_api.Models;

namespace map_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProxyController : ControllerBase
    {
        private readonly DalDbContext _DalDbContext;

        private readonly IConfiguration _Configuration;

        public ProxyController(DalDbContext _dalDbContext,
            IConfiguration _configuration)
        {
            _DalDbContext = _dalDbContext;

            _Configuration = _configuration;

            _GeoserverUrl = _Configuration["GeoserverUrl"];
        }

        #region properties and variables

        private static readonly SemaphoreSlim _SyncObject = new SemaphoreSlim(1);

        private static Dictionary<long, string> _EntityList = null;

        private static Dictionary<long, string> _LayerGeoserverNameList = null;

        private readonly string _GeoserverUrl;

        private Dictionary<long, string> LayerGeoserverNameList
        {
            get
            {
                if (EntityList != null) { }

                return _LayerGeoserverNameList;
            }
        }

        private Dictionary<long, string> EntityList
        {
            get
            {
                if (_EntityList == null)
                {
                    try
                    {
                        _SyncObject.Wait();

                        if (_EntityList == null)
                        {
                            var _query = _DalDbContext.LayerControl.
                                Select(d => new
                                {
                                    d.LayerId,
                                    d.EntityName,
                                    d.GeoserverAddress,
                                    d.GeoserverLayerName
                                });

                            var _layerDict = new Dictionary<long, string>();

                            var _layerGeoserverNameDict = new Dictionary<long, string>();

                            foreach (var _item in _query)
                            {
                                _layerDict.Add(_item.LayerId, _item.EntityName);

                                _layerGeoserverNameDict.Add(
                                    _item.LayerId,
                                    _item.GeoserverLayerName);
                            }

                            _EntityList = _layerDict;

                            _LayerGeoserverNameList = _layerGeoserverNameDict;
                        }
                    }
                    finally
                    {
                        _SyncObject.Release();
                    }
                }

                return _EntityList;
            }
        }

        #region params

        private const string _GetLegendGraphicLayersParam = "layers";

        private const string _LayersParam = "Layers";

        private const string _LayerIdParam = "layer_id";

        private const string _OpacityParam = "Opacities";

        private const string _CqlFilterParam = "CqlFilters";

        private const string VIEW_TYPE = "vıew_type";

        private const string VIEW_QUERY_TYPE = "view_query_type";

        #endregion

        #endregion

        #region actions

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetMap()
        {
            var _layerIds = ParameterCheck(
                HttpContext.Request.Query[_LayersParam].ToString().Split(',') ??
                new string[] { }).ToList();

            var _opacities = ParameterCheck(
                HttpContext.Request.Query[_OpacityParam].ToString().Split(',') ??
                new string[] { }).ToList();

            var _cqlFilters = GetCqlFilters();

            var _requestDictionary = new List<Dictionary<string, string>>();

            var _defaultList = new Dictionary<string, string>();

            for (int i = 0; i < _layerIds.Count(); i++)
            {
                Dictionary<string, string> _dict = null;

                var _opacity = _opacities.Count() > i ?
                    float.Parse(_opacities.ElementAt(i).Replace('.', ',') ?? "100") / 100.0 :
                    1.0f;

                if (_opacity == 1.0f)
                {
                    if (_defaultList.Count == 0)
                    {
                        _requestDictionary.Add(_defaultList);
                    }

                    _dict = _defaultList;
                }
                else
                {
                    _dict = new Dictionary<string, string>();
                }

                var _layerIdIndex = _layerIds.Count() > i ?
                    int.Parse(_layerIds.ElementAt(i) ?? "-1") :
                    -1;

                var _filter = _cqlFilters.Count() > i ?
                    (_cqlFilters.ElementAt(i) ?? "1=1") :
                    "1=1";

                _dict[_LayersParam] = (!_dict.ContainsKey(_LayersParam) ?
                    "" :
                    (_dict[_LayersParam] + ",")) + LayerGeoserverNameList[_layerIdIndex];

                _dict[_LayerIdParam] = (!_dict.ContainsKey(_LayerIdParam) ?
                    "" :
                    (_dict[_LayerIdParam] + ",")) + _layerIdIndex;

                _dict[_CqlFilterParam] = (!_dict.ContainsKey(_CqlFilterParam) ?
                    "" :
                    (_dict[_CqlFilterParam] + ";")) + _filter;

                _dict[_OpacityParam] = _opacity.ToString();

                if (_dict != _defaultList)
                {
                    _requestDictionary.Add(_dict);
                }
            }

            var _streams = new Stream[_requestDictionary.Count];

            var _opacityValues = new float[_requestDictionary.Count];

            var _tasks = _requestDictionary.Select(async _dictionary =>
            {
                try
                {
                    var _url = GetUrl();

                    var _queryStringArray = new string[] {
                        "layers=" + _dictionary[_LayersParam],
                        "cql_filter=" + _dictionary[_CqlFilterParam]
                    };

                    var _parametersCount = HttpUtility.ParseQueryString(_url).Count;

                    _url += ((_parametersCount == 0) ? "?" : "&") +
                        string.Join("&", _queryStringArray);

                    using var _client = new HttpClient();

                    var _response = await _client.GetAsync(_url);

                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        var _stream = await _response.Content.ReadAsStreamAsync();

                        lock (_requestDictionary)
                        {
                            var _index = _requestDictionary.IndexOf(_dictionary);

                            lock (_streams)
                            {
                                var _opacityIndex = _dictionary.First();

                                _streams[_index] = _stream;

                                _opacityValues[_index] =
                                    float.Parse(_dictionary[_OpacityParam]);
                            }
                        }
                    }
                }
                catch (Exception _ex)
                {
                    throw;
                }
            });

            await Task.WhenAll(_tasks);

            var _bytes = ImageToByte(CombineBitmap(_streams, _opacityValues));

            return File(_bytes, "image/png");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response> GetFeatureInfo()
        {
            var _result = new Response();

            try
            {
                var _url = GetUrl();

                var _name = string.Join(
                    ",",
                    HttpContext.Request.Query[_LayersParam].
                        ToString().
                        Split(',').
                        Select(l => LayerGeoserverNameList[int.Parse(l)]));

                var _queryStringArray = new string[] {
                    "QUERY_LAYERS=" + _name,
                    _LayersParam + "=" + _name,
                    _CqlFilterParam + "=" + GetCqlFilters().Aggregate((i,j)=> i + ";" + j)
                };

                var _parametersCount = HttpUtility.ParseQueryString(_url).Count;

                _url += ((_parametersCount == 0) ? "?" : "&") + string.Join("&", _queryStringArray);

                using var _client = new HttpClient();

                var _response = await _client.GetAsync(_url);

                var _content = await _response.Content.ReadAsStringAsync();

                _result.Data = GetFeatureInfoDetails(_content);

                _result.Success = true;

            }
            catch (Exception _ex)
            {
            }

            return _result;
        }

        [HttpGet]
        [Route("[action]/{_id}")]
        public async Task<IActionResult> GetLegend(int _id)
        {
            if (!LayerGeoserverNameList.ContainsKey(_id))
            {
                return NotFound();
            }

            var _name = LayerGeoserverNameList[_id];

            var _url = $"{_GeoserverUrl}request=getlegendgraphic&versıon=1.0.0&" +
                $"format=image/png&width=20&height=20&layer={_name}";

            using var _client = new HttpClient();

            var _response = await _client.GetAsync(_url);

            var _bytes = await _response.Content.ReadAsByteArrayAsync();

            return File(_bytes, "image/png");
        }

        #endregion

        #region utils

        private IEnumerable<string> GetCqlFilters()
        {
            try
            {
                return ParameterCheck(
                    HttpContext.Request.Query[_CqlFilterParam].
                    ToString().
                    Split(',') ?? new string[] { });
            }
            catch (Exception)
            {
                throw;
            }

        }
        private IEnumerable<string> ParameterCheck(string[] _parameter)
        {
            foreach (var _param in _parameter)
            {
                if (!string.IsNullOrEmpty(_param))
                {
                    yield return _param;
                }
            }
        }

        private string GetUrl()
        {
            var _result = "";

            try
            {
                var _ignored = new string[] {"url", "path", "async",
                    _GetLegendGraphicLayersParam, _LayerIdParam, _LayersParam, _OpacityParam,
                    _CqlFilterParam, VIEW_QUERY_TYPE,VIEW_TYPE };

                var _url = _GeoserverUrl;

                var _first = false;

                if (!_url.EndsWith("?"))
                {
                    _url += "?";
                }
                else
                {
                    _first = true;
                }

                foreach (var _key in HttpContext.Request.Query.Keys)
                {
                    if (_ignored.Contains(_key))
                        continue;

                    _url += (_first ? "" : "&") + _key + "=" +
                        HttpContext.Request.Query[_key].ToString();

                    _first = false;
                }

                _result = _url;
            }
            catch (Exception _ex)
            {
            }

            return _result;
        }

        private Bitmap CombineBitmap(Stream[] _streams, float[] _opacities)
        {
            Bitmap _result = null;

            var _images = new List<Bitmap>();

            try
            {
                var _width = 256;

                var _height = 256;

                foreach (var _image in _streams)
                {
                    var _bitmap = new Bitmap(_image);

                    _width = _bitmap.Width > _width ? _bitmap.Width : _width;

                    _height = _bitmap.Height > _height ? _bitmap.Height : _height;

                    _images.Add(_bitmap);
                }

                _result = new Bitmap(_width, _height);

                var _opacityIndex = 0;

                using (var _g = Graphics.FromImage(_result))
                {
                    _g.Clear(Color.Transparent);

                    foreach (var _image in _images)
                    {
                        var _opacity = _opacities[_opacityIndex++];

                        if (_opacity != 1f)
                        {
                            var _matrix = new ColorMatrix();

                            _matrix.Matrix33 = _opacity;

                            var _attributes = new ImageAttributes();

                            _attributes.SetColorMatrix(
                                _matrix,
                                ColorMatrixFlag.Default,
                                ColorAdjustType.Bitmap);

                            _g.DrawImage(_image,
                                new Rectangle(0, 0, _image.Width, _image.Height), 0, 0,
                                _image.Width, _image.Height, GraphicsUnit.Pixel, _attributes);
                        }
                        else
                        {
                            _g.DrawImage(_image,
                                new Rectangle(0, 0, _image.Width, _image.Height));
                        }
                    }
                }
                return _result;
            }
            catch (Exception _ex)
            {
                if (_result != null)
                {
                    _result.Dispose();
                }

                throw;
            }
            finally
            {
                foreach (var _image in _images)
                {
                    _image.Dispose();
                }
            }
        }
        private byte[] ImageToByte(Image _imgage)
        {
            var _converter = new ImageConverter();

            return (byte[])_converter.ConvertTo(_imgage, typeof(byte[]));
        }

        private IEnumerable<FeatureInfoDto> GetFeatureInfoDetails(string _response)
        {
            var _result = new List<FeatureInfoDto>();

            try
            {
                var _jobject = JObject.Parse(_response);

                var _features = _jobject["features"] as JArray;

                foreach (var _feature in _features)
                {
                    var _row = new FeatureInfoDto();

                    var _props = _feature["properties"];

                    var _featureId = (((JValue)(_feature["id"])).Value as string).
                        Split('.').
                        First();

                    var _id = LayerGeoserverNameList.
                        FirstOrDefault(x => x.Value == _featureId).Key;

                    var _entityName = EntityList[_id];

                    var _entityType = ReflectionTool.Instance.GetEntityType(_entityName);

                    var _infoAttributes = ReflectionTool.Instance.GetInfoAttributes(_entityType);

                    var _idAttribute = ReflectionTool.Instance.GetIdentityAttribute(_entityType);

                    _row.Id = _props[_idAttribute.Key.Name].ToString();

                    _row.IdColumn = _idAttribute.Value.PropertyName;

                    _row.Name = ReflectionTool.Instance.GetTableJsonAttribute(_entityType).Id;

                    var _properties = new Dictionary<string, string>();

                    foreach (var _infoAttribute in _infoAttributes)
                    {
                        _properties.Add(_infoAttribute.Value.PropertyName,
                            _props[_infoAttribute.Key.Name].ToString());
                    }

                    _row.Properties = _properties;

                    _result.Add(_row);
                }
            }
            catch (Exception _ex)
            {

            }

            return _result;
        }

        #endregion

    }
}
