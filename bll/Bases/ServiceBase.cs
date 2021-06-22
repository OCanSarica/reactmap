using dal.Bases;

namespace bll.Bases
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _UnitOfWork;

        public ServiceBase(IUnitOfWork _unitofWork)
        {
            _UnitOfWork = _unitofWork;
        }
    }
}
