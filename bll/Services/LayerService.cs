using bll.Bases;
using bll.Extensions;
using core.Models;
using core.Tools;
using dal.Bases;
using dal.Entities;
using dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace bll.Services
{
    public class LayerService : ServiceBase, ILayerService
    {
        private readonly IRepository<LayerControl> _LayerService;

        public LayerService(IUnitOfWork _unitofWork) : base(_unitofWork)
        {
            _LayerService = _unitofWork.GetRepository<LayerControl>();
        }

        public IList<LayerControlDto> GetLayerControl(long _userId)
        {
            var _layers = GetLayers(_userId);

            if (!_layers.Any())
            {
                _layers = GetLayers(-1);
            }

            return GetChildren(_layers, 0);
        }

        public IList<LayerDto> GetLayers(long _userId) =>
            _LayerService.
                GetAll(x => x.UserId == _userId && x.Status).
                OrderBy(x => x.Order).
                Select(x => x.ConverToDto()).
                ToList();

        public void Save() => _UnitOfWork.Save();

        private List<LayerControlDto> GetChildren(
            IList<LayerDto> _layers,
            long _layerId)
        {
            var _result = new List<LayerControlDto>();

            foreach (var _child in _layers.Where(x => x.ParentLayerId == _layerId))
            {
                _result.Add(new LayerControlDto
                {
                    Checked = _child.Checked,
                    children = GetChildren(_layers, _child.LayerId),
                    CqlFilter = _child.CqlFilter,
                    key = _child.LayerId,
                    MaxVisibleLevel = _child.MaxVisibleLevel,
                    MinVisibleLevel = _child.MinVisibleLevel,
                    Opacity = _child.Opacity,
                    ParentId = _child.ParentLayerId,
                    title = _child.Label
                });
            }

            return _result;
        }
    }
}
