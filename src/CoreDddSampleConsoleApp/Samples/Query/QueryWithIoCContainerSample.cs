using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Domain;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class QueryWithIoCContainerSample
    {
        public async Task QueryShipsByName()
        {
            var ioCContainer = new WindsorContainer();
            _RegisterComponents(ioCContainer);

            var unitOfWork = ioCContainer.Resolve<NhibernateUnitOfWork>();
            var shipRepository = new NhibernateRepository<Ship>(unitOfWork);

            try
            {
                unitOfWork.BeginTransaction();
                
                try
                {
                    var ship = new Ship("lady starlight", tonnage: 10m);
                    await shipRepository.SaveAsync(ship);

                    unitOfWork.Flush();

                    var queryExecutor = ioCContainer.Resolve<IQueryExecutor>();
                    var getShipByNameQuery = new GetShipsByNameQuery { ShipName = "lady" };
                    var shipDtos = await queryExecutor.ExecuteAsync<GetShipsByNameQuery, ShipDto>(getShipByNameQuery);

                    Console.WriteLine($"Ship by name query was executed by query executor resolved from IoC container. Number of ships queried: {shipDtos.Count()}");

                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
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
                Component.For<IQueryHandlerFactory>().AsFactory(), // register query handler factory (no real factory implementation needed :)
                Component.For<IQueryExecutor>() // register query executor
                    .ImplementedBy<QueryExecutor>()
                    .LifeStyle.Transient,
                Classes
                    .FromAssemblyContaining<GetShipsByNameQuery>() // register all query handlers in this assembly
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Component.For<INhibernateConfigurator>() // register nhibernate configurator
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifeStyle.Singleton,
                Component.For<NhibernateUnitOfWork>() // register nhibernate unit of work
                    .LifeStyle.PerThread
            );
        }
    }
}