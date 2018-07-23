using System;
using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.UnitOfWorks;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CommandWithCommandExecutorSample
    {
        public async Task CreateNewShip(CoreDddSampleNhibernateConfigurator nhibernateConfigurator)
        {
            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();
              
                try
                {
                    var commandExecutor = new CommandExecutor(new FakeCommandHandlerFactory(unitOfWork));
                    var createNewShipCommand = new CreateNewShipCommand {ShipName = "lady", Tonnage = 10m };

                    var generatedShipId = 0;
                    commandExecutor.CommandExecuted += args => generatedShipId = (int) args.Args;

                    await commandExecutor.ExecuteAsync(createNewShipCommand);

                    Console.WriteLine($"Create new ship command was executed by command executor. Generated ship id: {generatedShipId}");

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