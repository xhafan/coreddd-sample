using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Builders;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class MultipleQueriesSample
    {
        public async Task ExecuteMultipleQueries(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    await _BuildAndSaveEntities(unitOfWork);

                    var getPoliciesByTermsQuery = new GetPoliciesByTermsQuery { Terms = "one" };
                    var getPolicyItemsByShipNameQuery = new GetShipPolicyItemsByShipNameQuery{ ShipName = "some ship name" };

                    var policyDtos = await queryExecutor.ExecuteAsync<GetPoliciesByTermsQuery, PolicyDto>(getPoliciesByTermsQuery);
                    var shipPolicyItemDtos = await queryExecutor.ExecuteAsync<GetShipPolicyItemsByShipNameQuery, ShipPolicyItemDto>(getPolicyItemsByShipNameQuery);

                    // At this point, queries have not been sent to the DB. Only when the results are enumerated (below), 
                    // the queries are sent the the DB in one go, saving one round trip to the DB.

                    Console.WriteLine($"Policies by terms query was executed. Number of policy dtos queried: {policyDtos.Count()}");
                    Console.WriteLine($"Ship policy items by ship name query was executed. Number of ship policy item dtos queried: {shipPolicyItemDtos.Count()}");

                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task _BuildAndSaveEntities(NhibernateUnitOfWork unitOfWork)
        {
            var policyHolderRepository = new NhibernateRepository<PolicyHolder>(unitOfWork);
            var policyShipRepository = new NhibernateRepository<Ship>(unitOfWork);
            var policyRepository = new NhibernateRepository<Policy>(unitOfWork);


            var policyHolder = new PolicyHolderBuilder().Build();
            await policyHolderRepository.SaveAsync(policyHolder);


            var ship = new ShipBuilder().WithName("some ship name").Build();
            await policyShipRepository.SaveAsync(ship);


            var policyOne = new PolicyBuilder()
                .WithPolicyHolder(policyHolder)
                .WithTerms("policy one terms")
                .Build();
            await policyRepository.SaveAsync(policyOne);


            var policyTwoWithShip = new PolicyBuilder()
                .WithPolicyHolder(policyHolder)
                .WithShipPolicyItems(new ShipPolicyItemArgs
                {
                    Ship = ship,
                    InsuredTonnage = 7m,
                    RatePerTonnage = 5m
                })
                .Build();
            await policyRepository.SaveAsync(policyTwoWithShip);


            unitOfWork.Flush();
        }
    }
}