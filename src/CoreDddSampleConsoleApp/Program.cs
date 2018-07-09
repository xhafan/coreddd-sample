using System;
using System.Data.SQLite;
using CoreDddSampleConsoleApp.Samples;
using NHibernate.Tool.hbm2ddl;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        private static CoreDddSampleNhibernateConfigurator _nhibernateConfigurator;

        static void Main(string[] args)
        {
            _nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator();
            _CreateDatabase();

            new PersistNewEntitySample().PersistNewEntity(_nhibernateConfigurator);
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
