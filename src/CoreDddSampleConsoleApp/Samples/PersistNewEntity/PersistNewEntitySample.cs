using System;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
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

                var shipRepository = new NhibernateRepository<Ship>(unitOfWork);

                try
                {
                    var ship = new Ship("ship name", tonnage: 10m);
                    await shipRepository.SaveAsync(ship);

                    unitOfWork.Commit();

                    Console.WriteLine("Ship entity was persisted.");
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