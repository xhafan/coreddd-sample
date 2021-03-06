﻿using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddSampleWebAppCommon.Domain;

namespace CoreDddSampleWebAppCommon.Commands
{
    public class CreateNewShipCommandHandler : BaseCommandHandler<CreateNewShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public CreateNewShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

        public override async Task ExecuteAsync(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage);
            await _shipRepository.SaveAsync(newShip);
            
            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newShip.Id });
        }

        public override void Execute(CreateNewShipCommand command) // for WebForms app
        {
            ExecuteAsync(command).Wait();
        }
    }
}