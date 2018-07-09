using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples
{
    public class PersistNewEntitySample
    {
        public void PersistNewEntity(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var productRepository = new NhibernateRepository<Product>(unitOfWork);

                try
                {
                    var product = new Product();

                    productRepository.Save(product);

                    unitOfWork.Commit();
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