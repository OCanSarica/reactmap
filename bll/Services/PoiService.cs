using bll.Bases;
using bll.Extensions;
using core.Models;
using core.Tools;
using dal.Bases;
using dal.Entities;
using dal.Extensions;
using dto.Enums;
using dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace bll.Services
{
    public class PoiService : ServiceBase, IPoiService
    {
        private readonly IRepository<Poi> _PoiRepository;

        public PoiService(IUnitOfWork _unitofWork) : base(_unitofWork)
        {
            _PoiRepository = _unitofWork.GetRepository<Poi>();
        }

        public PoiDto Get(long _id) => _PoiRepository.Get(_id).ConvertToDto();

        public long Add(PoiDto _dto)
        {
            var _poi = _PoiRepository.Add(_dto.ConvertToEntity());

            Save();

            return _poi.Id;
        }

        public void Update(PoiDto _dto)
        {
            _PoiRepository.Update(_dto.ConvertToEntity());

            Save();
        }

        public void Remove(long _id)
        {
            _PoiRepository.Remove(_id);

            Save();
        }

        public PaginatorResponseDto<PoiDto> Paginate(PaginatorDto _dto)
        {
            var _rows = _PoiRepository.GetAll().
                OrderByDescending(x => x.Id).
                AsQueryable().
                Paginate(_dto, out int _count).
                ToListDynamic<Poi>().
                Select(x => x.ConvertToDto()).
                ToList();

            return new PaginatorResponseDto<PoiDto>
            {
                Count = _count,
                Rows = _rows
            };
        }

        public void Save() => _UnitOfWork.Save();
    }
}
