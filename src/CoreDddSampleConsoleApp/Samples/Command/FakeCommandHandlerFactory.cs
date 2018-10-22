using System;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    // This FakeCommandHandlerFactory would be implemented by IoC container out of the box in the real app. See CommandWithIoCContainerSample
    public class FakeCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public FakeCommandHandlerFactory(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        ICommandHandler<TCommand> ICommandHandlerFactory.Create<TCommand>()
        {
            if (typeof(TCommand) == typeof(CreateNewShipCommand))
            {
                return (ICommandHandler<TCommand>)new CreateNewShipCommandHandler(new NhibernateRepository<Ship>(_unitOfWork));
            }
            throw new Exception("Unsupported command");
        }

        public void Release<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand
        {
        }
    }
}