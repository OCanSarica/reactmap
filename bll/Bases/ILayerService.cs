using dal.Bases;
using dal.Entities;
using dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace bll.Bases
{
    public interface ILayerService : IService
    {
        IList<LayerControlDto> GetLayerControl(long _userId);

        IList<LayerDto> GetLayers(long _userId);
    }
}
