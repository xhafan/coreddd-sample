using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Dtos;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetPoliciesByTermsQueryHandler : BaseQueryOverHandler<GetPoliciesByTermsQuery>
    {
        public GetPoliciesByTermsQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetPoliciesByTermsQuery query)
        {
            return Session.QueryOver<PolicyDto>()
                          .WhereRestrictionOn(x => x.Terms)
                          .IsLike($"%{query.Terms}%");
        }
    }
}