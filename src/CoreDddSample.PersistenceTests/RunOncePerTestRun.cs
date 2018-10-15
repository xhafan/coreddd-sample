using System.Data.SQLite;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;
using CoreDddSampleCommon;
using NUnit.Framework;

namespace CoreDddSample.PersistenceTests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            _CreateDatabase();
        }

        private void _CreateDatabase()
        {
            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: false)) // shouldMapDtos: false -> make sure dto database views are not created as tables
            {
                var configuration = nhibernateConfigurator.GetConfiguration();
                var connectionString = configuration.Properties["connection.connection_string"];

                using (var connection = new SQLiteConnection(connectionString)) // for SQL server, use SqlConnection, for PostgreSQL, use NpgsqlConnection
                {
                    connection.Open();
                    new DatabaseSchemaCreator().CreateDatabaseSchema(nhibernateConfigurator, connection);
                }
            }
        }
    }
}