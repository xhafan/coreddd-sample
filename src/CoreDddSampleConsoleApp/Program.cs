using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples.MultipleQueries;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new DatabaseCreator().CreateDatabase();

            var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: true);

            await new PersistNewEntitySample().PersistNewEntity(nhibernateConfigurator);
            await new QuerySample().QueryShipsByName(nhibernateConfigurator);
            await new MultipleQueriesSample().ExecuteMultipleQueries(nhibernateConfigurator);
        }
    }
}
