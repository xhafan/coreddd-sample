using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class AddShipToPolicyCommandHandler : BaseCommandHandler<AddShipToPolicyCommand>
    {
        private readonly IRepository<Ship> _shipRepository;
        private readonly IRepository<Policy> _policyRepository;

        public AddShipToPolicyCommandHandler(
            IRepository<Ship> shipRepository,
            IRepository<Policy> policyRepository
        )
        {
            _policyRepository = policyRepository;
            _shipRepository = shipRepository;
        }

        public override async Task ExecuteAsync(AddShipToPolicyCommand command)
        {
            var ship = await _shipRepository.GetAsync(command.ShipId);
            var policy = await _policyRepository.GetAsync(command.PolicyId);

            policy.AddShipPolicyItem(new ShipPolicyItemArgs
            {
                Ship = ship,
                InsuredTonnage = command.InsuredTonnage,
                RatePerTonnage = command.RatePerTonnage
            });
        }
    }
}