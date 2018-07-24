using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Dtos;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class GetShipPolicyItemsByShipNameQueryHandler : BaseQueryOverHandler<GetShipPolicyItemsByShipNameQuery>
    {
        public GetShipPolicyItemsByShipNameQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetShipPolicyItemsByShipNameQuery query)
        {
            return Session.QueryOver<ShipPolicyItemDto>()
                          .WhereRestrictionOn(x => x.ShipName)
                          .IsLike($"%{query.ShipName}%");
        }
    }
}