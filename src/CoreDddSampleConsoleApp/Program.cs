using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples.Command;
using CoreDddSampleConsoleApp.Samples.MultipleQueries;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;
using HibernatingRhinos.Profiler.Appender.NHibernate;

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
                await new MultipleQueriesSample().ExecuteMultipleQueries(nhibernateConfigurator);

                await new CommandSample().CreateNewShip(nhibernateConfigurator);
                await new CommandWithCommandExecutorSample().CreateNewShip(nhibernateConfigurator);

            }

            await new QueryWithIoCContainerSample().QueryShipsByName();
            await new QueryWithIoCContainerAndQueryExecutorDependencyInjectionSample().QueryShipsByName();

            await new CommandWithIoCContainerSample().CreateNewShip();
            await new CommandWithIoCContainerAndCommandExecutorDependencyInjectionSample().CreateNewShip();
            await new CommandWithDomainBehaviourExecutedOnExistingAggregateRootEntitySample().UpdateShipData();
        }
    }
}
