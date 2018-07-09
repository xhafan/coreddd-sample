using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class AllProductsQueryHandler : BaseQueryOverHandler<AllProductsQuery>
    {
        public AllProductsQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(AllProductsQuery query)
        {
            return Session.QueryOver<Product>();
        }
    }
}