using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Domain.Repositories;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QueryWithIoCContainerAndQueryExecutorDependencyInjectionSample
    {
        public async Task QueryShipsByName()
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();
            var shipRepository = ioCContainer.Resolve<IRepository<Ship>>();

            try
            {
                unitOfWork.BeginTransaction();

                try
                {
                    var ship = new Ship("lady starlight", tonnage: 10m);
                    await shipRepository.SaveAsync(ship);

                    unitOfWork.Flush();

                    var shipController = ioCContainer.Resolve<ShipController>();
                    var shipDtos = await shipController.GetShipsByNameAsync(shipName: "lady");

                    Console.WriteLine($"Ship by name query was executed by query executor injected into ShipController. Number of ships queried: {shipDtos.Count()}");

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
                    .FromAssemblyContaining<GetShipsByNameQuery>() // register all query handlers in this assembly
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<ShipController>() // register ship controller to get query executor injected into the constructor
            );
        }
    }
}