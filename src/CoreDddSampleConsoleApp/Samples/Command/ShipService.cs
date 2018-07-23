using System.Threading.Tasks;
using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class ShipService
    {
        private readonly ICommandExecutor _commandExecutor;

        public ShipService(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        public async Task<int> CreateNewShipAsync(string shipName, decimal tonnage)
        {
            var createNewShipCommand = new CreateNewShipCommand {ShipName = shipName, Tonnage = tonnage};

            var generatedShipId = 0;
            _commandExecutor.CommandExecuted += args => generatedShipId = (int)args.Args;

            await _commandExecutor.ExecuteAsync(createNewShipCommand);

            return generatedShipId;
        }

        public async Task UpdateShipData(int shipId, string shipName, decimal tonnage)
        {
            var updateShipDataCommand = new UpdateShipDataCommand
            {
                ShipId = shipId,
                ShipName = shipName,
                Tonnage = tonnage
            };
            await _commandExecutor.ExecuteAsync(updateShipDataCommand);
        }
    }
}