using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples.Command;
using CoreDddSampleConsoleApp.Samples.Ddd;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new DatabaseCreator().CreateDatabase();

            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator())
            {
                await new PersistNewEntitySample().PersistNewEntity(nhibernateConfigurator);

                await new QuerySample().QueryShipsByName(nhibernateConfigurator);
                await new QueryWithQueryExecutorSample().QueryShipsByName(nhibernateConfigurator);

                await new CommandSample().CreateNewShip(nhibernateConfigurator);
                await new CommandWithCommandExecutorSample().CreateNewShip(nhibernateConfigurator);

                await new DddSample()
                    .BuildAndPersistPolicyEntitiesAndExecuteDomainBehaviourOnThemAndExecuteQueriesOverThem(
                        nhibernateConfigurator,
                        isDelayedDomainEventHandlingEnabled: false // immediate domain event handling when raised
                        );
                await new DddSample()
                    .BuildAndPersistPolicyEntitiesAndExecuteDomainBehaviourOnThemAndExecuteQueriesOverThem(
                        nhibernateConfigurator,
                        isDelayedDomainEventHandlingEnabled: true // delayed domain event handling, domain event handlers are executed manually by calling DomainEvents.RaiseDelayedEvents();
                    );
            }

            await new QueryWithIoCContainerSample().QueryShipsByName();
            await new QueryWithIoCContainerAndQueryExecutorDependencyInjectionSample().QueryShipsByName();
            await new QueryWithBatchingSample().QueryShipsByMultipleNames();

            await new CommandWithIoCContainerSample().CreateNewShip();
            await new CommandWithIoCContainerAndCommandExecutorDependencyInjectionSample().CreateNewShip();
            await new CommandWithDomainBehaviourExecutedOnExistingAggregateRootEntitySample().UpdateShipData();
        }
    }
}
