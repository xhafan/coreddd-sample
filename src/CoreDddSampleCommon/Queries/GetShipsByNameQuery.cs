using CoreDdd.Queries;

namespace CoreDddSampleCommon.Queries
{
    public class GetShipsByNameQuery : IQuery
    {
        public string ShipName { get; set; }
    }
}