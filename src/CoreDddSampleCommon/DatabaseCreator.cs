using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;
using CoreDddSampleWebAppCommon.Dtos;

namespace CoreDddSampleWebAppCommon
{
    public class DatabaseCreator
    {
        public void CreateDatabase()
        {
            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: false)) // shouldMapDtos: false -> make sure dto database views are not created as tables
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
            _CreateDatabaseView(connection, "ShipDto.sql");
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
            var shipDtoType = typeof(ShipDto);
            var assembly = shipDtoType.Assembly;
            using (var stream = assembly.GetManifestResourceStream($"CoreDddSampleCommon.DatabaseViews.{resourceName}"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}