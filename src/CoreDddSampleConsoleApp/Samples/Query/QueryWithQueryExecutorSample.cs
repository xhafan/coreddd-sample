using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QueryWithQueryExecutorSample
    {
        public async Task QueryShipsByName(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var shipRepository = new NhibernateRepository<Ship>(unitOfWork);
                var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(unitOfWork));

                try
                {
                    var ship = new Ship("lady starlight", tonnage: 10m);
                    await shipRepository.SaveAsync(ship);

                    unitOfWork.Flush();

                    var getShipByNameQuery = new GetShipsByNameQuery {ShipName = "lady"};
                    var shipDtos = await queryExecutor.ExecuteAsync<GetShipsByNameQuery, ShipDto>(getShipByNameQuery);

                    Console.WriteLine($"Ship by name query was executed by query executor. Number of ships queried: {shipDtos.Count()}");

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