using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreDddSampleWebAppCommon.Commands;
using CoreDddSampleWebAppCommon.Dtos;
using CoreDddSampleWebAppCommon.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CoreDddSampleAspNetCoreWebApp.Controllers
{
    public class ShipController : Controller
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public ShipController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor
        )
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        public async Task<string> CreateNewShip(string shipName, decimal tonnage)
        {
            var generatedShipId = 0;
            _commandExecutor.CommandExecuted += args => generatedShipId = (int)args.Args;

            await _commandExecutor.ExecuteAsync(new CreateNewShipCommand { ShipName = shipName, Tonnage = tonnage });

            return $"A new ship was created. ship id: {generatedShipId}";
        }

        public async Task<string> GetShipsByName(string shipName)
        {
            var shipDtos = (await _queryExecutor.ExecuteAsync<GetShipsByNameQuery, ShipDto>(new GetShipsByNameQuery { ShipName = shipName })).ToList();

            var info = $"Number of ships queried: {shipDtos.Count}\n";
            foreach (var shipDto in shipDtos)
            {
                info += $"Id: {shipDto.Id}, ship name: {shipDto.Name}\n";
            }
            return info;
        }

        public async Task<string> UpdateShipData(int shipId, string shipName, decimal tonnage)
        {
            await _commandExecutor.ExecuteAsync(new UpdateShipDataCommand { ShipId = shipId, ShipName = shipName, Tonnage = tonnage });

            return "Ship data updated.";
        }
    }
}