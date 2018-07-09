using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetProductByNameQueryHandler : BaseQueryOverHandler<GetProductByNameQuery>
    {
        public GetProductByNameQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetProductByNameQuery query)
        {
            return Session.QueryOver<Product>()
                          .WhereRestrictionOn(x => x.Name)
                          .IsLike($"%{query.Name}%");
        }
    }
}