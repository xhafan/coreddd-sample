using System;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QuerySample
    {
        public async Task QueryShipsByName(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var shipRepository = new NhibernateRepository<Ship>(unitOfWork);
                
                try
                {
                    var ship = new Ship("lady starlight", tonnage: 10m);
                    await shipRepository.SaveAsync(ship);

                    unitOfWork.Flush();

                    var shipByNameQuery = new GetShipsByNameQuery {ShipName = "lady"};
                    var shipByNameQueryHandler = new GetShipsByNameQueryHandler(unitOfWork);

                    var shipDtos = await shipByNameQueryHandler.ExecuteAsync<ShipDto>(shipByNameQuery);

                    Console.WriteLine($"Ship by name query was executed. Number of ships queried: {shipDtos.Count()}");

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