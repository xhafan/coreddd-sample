using System;
using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CommandSample
    {
        public async Task CreateNewShip(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var shipRepository = new NhibernateRepository<Ship>(unitOfWork);
                
                try
                {
                    var createNewShipCommand = new CreateNewShipCommand {ShipName = "lady", Tonnage = 10m };
                    var createNewShipCommandHandler = new CreateNewShipCommandHandler(shipRepository);

                    var generatedShipId = 0;
                    createNewShipCommandHandler.CommandExecuted += args => generatedShipId = (int) args.Args;

                    await createNewShipCommandHandler.ExecuteAsync(createNewShipCommand);

                    Console.WriteLine($"Create new ship command was executed. Generated ship id: {generatedShipId}");

                    await unitOfWork.CommitAsync();
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }
    }
}