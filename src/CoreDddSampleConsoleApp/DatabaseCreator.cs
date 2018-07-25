using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NHibernate.Tool.hbm2ddl;

namespace CoreDddSampleConsoleApp
{
    public class DatabaseCreator
    {
        public async Task CreateDatabase()
        {
            using (var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: false))
            {
                var configuration = nhibernateConfigurator.GetConfiguration();
                var connectionString = configuration.Properties["connection.connection_string"];

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    await new SchemaExport(configuration).ExecuteAsync(
                        useStdOut: true,
                        execute: true,
                        justDrop: false,
                        connection: connection,
                        exportOutput: null
                    );
                    await _CreateDtoViews(connection);
                }
            }                
        }

        private async Task _CreateDtoViews(DbConnection connection)
        {
            await _CreateDatabaseView(connection, "PolicyDto.sql");
            await _CreateDatabaseView(connection, "ShipDto.sql");
            await _CreateDatabaseView(connection, "ShipCargoPolicyItemDto.sql");
        }

        private async Task _CreateDatabaseView(DbConnection connection, string databaseViewFileName)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = await _ReadDatabaseViewEmbeddedResource(databaseViewFileName);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task<string> _ReadDatabaseViewEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"{GetType().Namespace}.DatabaseViews.{resourceName}"))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}