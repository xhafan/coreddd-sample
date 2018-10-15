using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;

namespace CoreDddSampleConsoleApp
{
    public class DatabaseCreator
    {
        public void CreateDatabase()
        {
            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: false))
            {
                var configuration = nhibernateConfigurator.GetConfiguration();
                var connectionString = configuration.Properties["connection.connection_string"];

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    new DatabaseSchemaCreator().CreateDatabaseSchema(nhibernateConfigurator, connection);
                    _CreateDtoViews(connection);
                }
            }                
        }

        private void _CreateDtoViews(DbConnection connection)
        {
            _CreateDatabaseView(connection, "PolicyDto.sql");
            _CreateDatabaseView(connection, "ShipDto.sql");
            _CreateDatabaseView(connection, "ShipCargoPolicyItemDto.sql");
        }

        private void _CreateDatabaseView(DbConnection connection, string databaseViewFileName)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = _ReadDatabaseViewEmbeddedResource(databaseViewFileName);
                cmd.ExecuteNonQuery();
            }
        }

        private string _ReadDatabaseViewEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"{GetType().Namespace}.DatabaseViews.{resourceName}"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}