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

                var productRepository = new NhibernateRepository<Product>(unitOfWork);

                try
                {
                    var product = new Product("product name", "product description", 10m);

                    await productRepository.SaveAsync(product);

                    unitOfWork.Commit();

                    Console.WriteLine("Product entity was persisted.");
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