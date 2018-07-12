using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDdd.Queries;
using CoreDddSampleConsoleApp.Dtos;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    // An example controller similar to ASP.NET Core controller to demonstrate query executor usage
    public class ShipController
    {
        private readonly IQueryExecutor _queryExecutor;

        public ShipController(IQueryExecutor queryExecutor)
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