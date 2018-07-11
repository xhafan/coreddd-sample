﻿using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using CoreDddSampleConsoleApp.Samples.MultipleQueries;
using CoreDddSampleConsoleApp.Samples.PersistNewEntity;
using CoreDddSampleConsoleApp.Samples.Query;
using NHibernate.Tool.hbm2ddl;

namespace CoreDddSampleConsoleApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            await _CreateDatabase();

            var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: true);
            await new PersistNewEntitySample().PersistNewEntity(nhibernateConfigurator);
            await new QuerySample().QueryAllPolicies(nhibernateConfigurator);
            await new MultipleQueriesSample().ExecuteMultipleQueries(nhibernateConfigurator);
        }

        private static async Task _CreateDatabase()
        {
            var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator(shouldMapDtos: false);
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
                        exportOutput: Console.Out)
                    ;
                await _CreateDtoViews(connection);
                await _CreateShipPolicyItemDtoView(connection);
            }
        }

        private static async Task _CreateDtoViews(DbConnection connection)
        {
            await _CreatePolicyDtoView(connection);
        }

        private static async Task _CreatePolicyDtoView(DbConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
drop view if exists PolicyDto;

create view PolicyDto
as
select 
	Id as PolicyId
    , Terms
from Policy 
";
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async Task _CreateShipPolicyItemDtoView(DbConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
drop view if exists ShipPolicyItemDto;

create view ShipPolicyItemDto
as
select 
	item.Id as PolicyItemId
    , p.Id as PolicyId
    , shipItem.ShipId
    , s.Name     as ShipName
from Policy p
join PolicyItem item on item.PolicyId = p.Id
join CargoPolicyItem cargoItem on cargoItem.PolicyItemId = item.Id
join ShipPolicyItem shipItem on shipItem.CargoPolicyItemId = item.Id
join Ship s on s.Id = shipItem.ShipId
";
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}
