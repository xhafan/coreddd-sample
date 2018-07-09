using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QuerySample
    {
        public async Task QueryAllProducts(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var productRepository = new NhibernateRepository<Product>(unitOfWork);
                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    var product = new Product("product name", "product description", 10m);
                    await productRepository.SaveAsync(product);
                    unitOfWork.Flush();

                    var allProducts = await queryExecutor.ExecuteAsync<AllProductsQuery, Product>(new AllProductsQuery());

                    Console.WriteLine($"All products query was executed. Number of product entities queried: {allProducts.Count()}");

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