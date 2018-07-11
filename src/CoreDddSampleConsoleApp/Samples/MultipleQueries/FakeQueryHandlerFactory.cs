﻿using System;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
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
            if (typeof(TQuery) == typeof(GetPoliciesByTermsQuery))
            {
                return (IQueryHandler<TQuery>)new GetPoliciesByTermsQueryHandler(_unitOfWork);
            }
            if (typeof(TQuery) == typeof(GetShipPolicyItemsByShipNameQuery))
            {
                return (IQueryHandler<TQuery>)new GetShipPolicyItemsByShipNameQueryHandler(_unitOfWork);
            }

            throw new Exception("Unsupported query");
        }

        public void Release<TQuery>(IQueryHandler<TQuery> queryHandler) where TQuery : IQuery
        {
        }
    }
}