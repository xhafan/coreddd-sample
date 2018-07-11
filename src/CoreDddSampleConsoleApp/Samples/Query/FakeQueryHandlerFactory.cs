using System;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    // This FakeQueryHandlerFactory is implemented by IoC container in the real app. See the example todo
    public class FakeQueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public FakeQueryHandlerFactory(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryHandler<TQuery> Create<TQuery>() where TQuery : IQuery
        {
            if (typeof(TQuery) == typeof(AllPoliciesQuery))
            {
                return (IQueryHandler<TQuery>)new AllPoliciesQueryHandler(_unitOfWork);
            }
            throw new Exception("Unsupported query");
        }

        public void Release<TQuery>(IQueryHandler<TQuery> queryHandler) where TQuery : IQuery
        {
        }
    }
}