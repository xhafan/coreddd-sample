using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleAspNetCoreWebApp.Dtos;
using NHibernate;

namespace CoreDddSampleAspNetCoreWebApp.Queries
{
    public class GetShipsByNameQueryHandler : BaseQueryOverHandler<GetShipsByNameQuery>
    {
        public GetShipsByNameQueryHandler(NhibernateUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetShipsByNameQuery query)
        {
            return Session.QueryOver<ShipDto>()
                          .WhereRestrictionOn(x => x.Name)
                          .IsLike($"%{query.ShipName}%");
        }
    }
}