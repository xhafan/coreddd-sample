using System;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    // This FakeQueryHandlerFactory would be implemented by IoC container out of the box in the real app. See QueryWithIoCContainerSample
    public class FakeQueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public FakeQueryHandlerFactory(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryHandler<TQuery> Create<TQuery>() where TQuery : IQuery
        {
            if (typeof(TQuery) == typeof(GetShipsByNameQuery))
            {
                return (IQueryHandler<TQuery>)new GetShipsByNameQueryHandler(_unitOfWork);
            }
            throw new Exception("Unsupported query");
        }

        public void Release<TQuery>(IQueryHandler<TQuery> queryHandler) where TQuery : IQuery
        {
        }
    }
}