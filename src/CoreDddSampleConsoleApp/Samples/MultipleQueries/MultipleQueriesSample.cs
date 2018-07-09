using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class MultipleQueriesSample
    {
        public async Task ExecuteMultipleQueries(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var productRepository = new NhibernateRepository<Product>(unitOfWork);
                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    var productOne = new Product("product one name", "product one description", 10m);
                    var productTwo = new Product("product two name", "product two description", 20m);
                    await productRepository.SaveAsync(productOne);
                    await productRepository.SaveAsync(productTwo);
                    unitOfWork.Flush(); 

                    var getProductWithOneInNameQuery = new GetProductByNameQuery { Name = "one" };
                    var getProductWithTwoInDescriptionQuery = new GetProductByDescriptionQuery{ Description = "two" };

                    var productsByName = await queryExecutor.ExecuteAsync<GetProductByNameQuery, Product>(getProductWithOneInNameQuery);
                    var productsByDescription = await queryExecutor.ExecuteAsync<GetProductByDescriptionQuery, Product>(getProductWithTwoInDescriptionQuery);

                    // At this point, queries have not been sent to the DB. Only when the results are enumerated (below), 
                    // the queries are sent the the DB in one go, saving one round trip to the DB.

                    Console.WriteLine($"Products by name query was executed. Number of product entities queried: {productsByName.Count()}");
                    Console.WriteLine($"Products by description query was executed. Number of product entities queried: {productsByDescription.Count()}");

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