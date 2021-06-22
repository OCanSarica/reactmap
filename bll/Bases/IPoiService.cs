using core.Models;
using dto.Models;

namespace bll.Bases
{
    public interface IPoiService : IService
    {
        PoiDto Get(long _id);

        long Add(PoiDto _dto);

        void Update(PoiDto _dto);

        void Remove(long _id);

        PaginatorResponseDto<PoiDto> Paginate(PaginatorDto _dto);
    }
}
