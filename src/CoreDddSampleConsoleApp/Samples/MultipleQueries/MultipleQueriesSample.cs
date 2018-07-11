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

                var policyRepository = new NhibernateRepository<Policy>(unitOfWork);
                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    var policyHolder = await _BuildAndSavePolicyHolder(unitOfWork);

                    var policyOne = new PolicyBuilder().WithPolicyHolder(policyHolder).WithTerms("policy one terms").Build();

                    var ship = await _BuildAndSaveShip(unitOfWork, shipName: "some ship name");
                    var policyTwoWithShip = new PolicyBuilder()
                        .WithPolicyHolder(policyHolder)
                        .WithShipPolicyItems(new ShipPolicyItemArgs
                        {
                            Ship = ship,
                            InsuredTonnage = 7m,
                            RatePerTonnage = 5m
                        })
                        .Build();
                    await policyRepository.SaveAsync(policyOne);
                    await policyRepository.SaveAsync(policyTwoWithShip);

                    unitOfWork.Flush(); 

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

        private async Task<PolicyHolder> _BuildAndSavePolicyHolder(NhibernateUnitOfWork unitOfWork)
        {
            var policyHolder = new PolicyHolderBuilder().Build();
            await new NhibernateRepository<PolicyHolder>(unitOfWork).SaveAsync(policyHolder);
            return policyHolder;
        }

        private async Task<Ship> _BuildAndSaveShip(NhibernateUnitOfWork unitOfWork, string shipName)
        {
            var ship = new ShipBuilder().WithName(shipName).Build();
            await new NhibernateRepository<Ship>(unitOfWork).SaveAsync(ship);
            return ship;
        }
    }
}