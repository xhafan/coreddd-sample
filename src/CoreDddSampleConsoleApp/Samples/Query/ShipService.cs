using System.Collections.Generic;
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
    }
}