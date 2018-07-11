using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Query
{
    public class GetShipsByNameQuery : IQuery
    {
        public string ShipName { get; set; }
    }
}