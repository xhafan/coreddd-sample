using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetProductByDescriptionQueryHandler : BaseQueryOverHandler<GetProductByDescriptionQuery>
    {
        public GetProductByDescriptionQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetProductByDescriptionQuery query)
        {
            return Session.QueryOver<Product>()
                          .WhereRestrictionOn(x => x.Description)
                          .IsLike($"%{query.Description}%");
        }
    }
}