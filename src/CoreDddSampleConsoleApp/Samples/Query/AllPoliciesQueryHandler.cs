using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Dtos;
using NHibernate;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class AllPoliciesQueryHandler : BaseQueryOverHandler<AllPoliciesQuery>
    {
        public AllPoliciesQueryHandler(NhibernateUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(AllPoliciesQuery query)
        {
            return Session.QueryOver<PolicyDto>();
        }
    }
}