using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples;
using CoreDddSampleConsoleApp.Samples.MultipleQueries;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;
using NHibernate.Tool.hbm2ddl;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        private static CoreDddSampleNhibernateConfigurator _nhibernateConfigurator;

        static async Task Main(string[] args)
        {
            _nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator();
            _CreateDatabase();

            await new PersistNewEntitySample().PersistNewEntity(_nhibernateConfigurator);
            await new QuerySample().QueryAllProducts(_nhibernateConfigurator);
            await new MultipleQueriesSample().ExecuteMultipleQueries(_nhibernateConfigurator);
        }

        private static void _CreateDatabase()
        {
            var configuration = _nhibernateConfigurator.GetConfiguration();
            var connectionString = configuration.Properties["connection.connection_string"];

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                new SchemaExport(configuration).Execute(
                        useStdOut: true,
                        execute: true,
                        justDrop: false,
                        connection: connection,
                        exportOutput: Console.Out)
                    ;
            }
        }
    }
}
