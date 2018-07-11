using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Dtos;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class ShipByNameQueryHandler : BaseQueryOverHandler<ShipByNameQuery>
    {
        public ShipByNameQueryHandler(NhibernateUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(ShipByNameQuery query)
        {
            return Session.QueryOver<ShipDto>()
                          .WhereRestrictionOn(x => x.Name)
                          .IsLike($"%{query.ShipName}%");
        }
    }
}