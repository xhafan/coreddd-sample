using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Builders;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QuerySample
    {
        public async Task QueryAllPolicies(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    await _BuildAndSaveEntities(unitOfWork);

                    var allPolicies = await queryExecutor.ExecuteAsync<AllPoliciesQuery, PolicyDto>(new AllPoliciesQuery());

                    Console.WriteLine($"All policies query was executed. Number of policy entities queried: {allPolicies.Count()}");

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
            var policyRepository = new NhibernateRepository<Policy>(unitOfWork);

            var policyHolder = new PolicyHolderBuilder().Build();
            await policyHolderRepository.SaveAsync(policyHolder);

            var policy = new PolicyBuilder().WithPolicyHolder(policyHolder).Build();
            await policyRepository.SaveAsync(policy);

            unitOfWork.Flush();
        }
    }
}