using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class ShipService
    {
        private readonly IQueryExecutor _queryExecutor;

        public ShipService(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public async Task<IEnumerable<ShipDto>> GetShipsByNameAsync(string shipName)
        {
            var getShipByNameQuery = new GetShipsByNameQuery { ShipName = shipName };
            return await _queryExecutor.ExecuteAsync<GetShipsByNameQuery, ShipDto>(getShipByNameQuery);
        }

        public async Task<IEnumerable<ShipDto>> GetShipsByMultipleNames(
            string shipNameOne,
            string shipNameTwo
        )
        {
            var getShipByNameOneQuery = new GetShipsByNameQuery { ShipName = shipNameOne };
            var shipsByNameOne = _queryExecutor.Execute<GetShipsByNameQuery, ShipDto>(getShipByNameOneQuery);

            // At this point, getShipByNameOneQuery have not been sent to the DB. 
            // The query would be sent to the DB only when the result (shipsByNameOne) is enumerated.

            var getShipByNameTwoQuery = new GetShipsByNameQuery { ShipName = shipNameTwo };
            var shipsByNameTwo = await _queryExecutor.ExecuteAsync<GetShipsByNameQuery, ShipDto>(getShipByNameTwoQuery);

            // getShipByNameTwoQuery async execution was awaited, which sent both queries to the DB in the single round trip.

            return shipsByNameOne.Union(shipsByNameTwo);
        }
    }
}