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

                var policyHolderRepository = new NhibernateRepository<PolicyHolder>(unitOfWork);

                try
                {
                    var policyHolder = new PolicyHolderBuilder().Build();
                    await policyHolderRepository.SaveAsync(policyHolder);

                    unitOfWork.Commit();

                    Console.WriteLine("PolicyHolder entity was persisted.");
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