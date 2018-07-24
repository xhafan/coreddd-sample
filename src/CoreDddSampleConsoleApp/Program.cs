using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples.Command;
using CoreDddSampleConsoleApp.Samples.ComplexDdd;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new DatabaseCreator().CreateDatabase();

            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator())
            {
                await new PersistNewEntitySample().PersistNewEntity(nhibernateConfigurator);

                await new QuerySample().QueryShipsByName(nhibernateConfigurator);
                await new QueryWithQueryExecutorSample().QueryShipsByName(nhibernateConfigurator);

                await new CommandSample().CreateNewShip(nhibernateConfigurator);
                await new CommandWithCommandExecutorSample().CreateNewShip(nhibernateConfigurator);

                await new ComplexDddSample().BuildAndPersistPolicyEntitiesAndExecuteDomainBehaviourOnThemAndExecuteQueriesOverThem(nhibernateConfigurator);
            }

            await new QueryWithIoCContainerSample().QueryShipsByName();
            await new QueryWithIoCContainerAndQueryExecutorDependencyInjectionSample().QueryShipsByName();

            await new CommandWithIoCContainerSample().CreateNewShip();
            await new CommandWithIoCContainerAndCommandExecutorDependencyInjectionSample().CreateNewShip();
            await new CommandWithDomainBehaviourExecutedOnExistingAggregateRootEntitySample().UpdateShipData();
        }
    }
}
