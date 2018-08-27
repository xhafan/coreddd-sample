using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Register.Castle;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CommandWithIoCContainerAndCommandExecutorDependencyInjectionSample
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
                    var shipService = ioCContainer.Resolve<ShipService>();
                    var generatedShipId = await shipService.CreateNewShipAsync(shipName: "lady", tonnage: 10m);

                    Console.WriteLine($"Create new ship command was executed by command executor injected into ShipService. Generated ship id: {generatedShipId}");

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
            NhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerThread);

            ioCContainer.Install(
                FromAssembly.Containing<QueryExecutorInstaller>(),
                FromAssembly.Containing<NhibernateInstaller>()
            );

            ioCContainer.Register(
                Classes
                    .FromAssemblyContaining<CreateNewShipCommand>() // register all command handlers in this assembly
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<ShipService>() // register ship service to get command executor injected into the constructor
            );
        }
    }
}