using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Dtos;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class GetShipCargoPolicyItemsByShipNameQueryHandler : BaseQueryOverHandler<GetShipCargoPolicyItemsByShipNameQuery>
    {
        public GetShipCargoPolicyItemsByShipNameQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetShipCargoPolicyItemsByShipNameQuery query)
        {
            return Session.QueryOver<ShipCargoPolicyItemDto>()
                          .WhereRestrictionOn(x => x.ShipName)
                          .IsLike($"%{query.ShipName}%");
        }
    }
}