using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDddSampleConsoleApp.Samples.Command;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class ShipController
    {
        private readonly ICommandExecutor _commandExecutor;

        public ShipController(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public async Task<int> CreateNewShipAsync(string name, decimal tonnage)
        {
            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = name,
                Tonnage = tonnage
            };
            var newShipId = 0;
            _commandExecutor.CommandExecuted += args => newShipId = (int) args.Args;

            await _commandExecutor.ExecuteAsync(createNewShipCommand);

            return newShipId;
        }
    }
}