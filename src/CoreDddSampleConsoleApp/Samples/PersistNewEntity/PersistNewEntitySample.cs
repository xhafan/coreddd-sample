using System;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Builders;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.PersistNewEntity
{
    public class PersistNewEntitySample
    {
        public async Task PersistNewEntity(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                try
                {
                    var policyHolder = new PolicyHolderBuilder().Build();
                    await new NhibernateRepository<PolicyHolder>(unitOfWork).SaveAsync(policyHolder);

                    var policy = new PolicyBuilder().WithPolicyHolder(policyHolder).Build();                
                    await new NhibernateRepository<Policy>(unitOfWork).SaveAsync(policy);

                    unitOfWork.Commit();

                    Console.WriteLine("Policy entity was persisted.");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}