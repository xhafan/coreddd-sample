using System;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CommandWithIoCContainerSample
    {
        public async Task CreateNewShip()
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();

            try
            {
                unitOfWork.BeginTransaction();
                
                try
                {
                    var commandExecutor = ioCContainer.Resolve<ICommandExecutor>();
                    var createNewShipCommand = new CreateNewShipCommand { ShipName = "lady", Tonnage = 10m };

                    var generatedShipId = 0;
                    commandExecutor.CommandExecuted += args => generatedShipId = (int)args.Args;

                    await commandExecutor.ExecuteAsync(createNewShipCommand);

                    Console.WriteLine($"Create new ship command was executed by command executor resolved from IoC container. Generated ship id: {generatedShipId}");

                    await unitOfWork.CommitAsync();
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
            finally
            {
                ioCContainer.Release(unitOfWork);
            }

            ioCContainer.Dispose();
        }

        private void _RegisterComponents(WindsorContainer ioCContainer)
        {
            ioCContainer.AddFacility<TypedFactoryFacility>();

            ioCContainer.Register(
                Component.For<ICommandHandlerFactory>().AsFactory(), // register command handler factory (no real factory implementation needed :)
                Component.For<ICommandExecutor>() // register command executor
                    .ImplementedBy<CommandExecutor>()
                    .LifeStyle.Transient,
                Classes
                    .FromAssemblyContaining<CreateNewShipCommand>() // register all command handlers in this assembly
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<NhibernateUnitOfWork>() // register nhibernate unit of work
                    .LifeStyle.PerThread,
                Component.For(typeof(IRepository<>)) // register repositories
                    .ImplementedBy(typeof(NhibernateRepository<>))
                    .LifeStyle.Transient
            );
        }
    }
}