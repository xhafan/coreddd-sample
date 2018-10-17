using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddSampleWebAppCommon.Domain;

namespace CoreDddSampleWebAppCommon.Commands
{
    public class UpdateShipDataCommandHandler : BaseCommandHandler<UpdateShipDataCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public UpdateShipDataCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

        public override async Task ExecuteAsync(UpdateShipDataCommand command)
        {
            var ship = await _shipRepository.GetAsync(command.ShipId);

            ship.UpdateData(command.ShipName, command.Tonnage);
        }
    }
}