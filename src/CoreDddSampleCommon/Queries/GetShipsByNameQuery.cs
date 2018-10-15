using CoreDdd.Queries;

namespace CoreDddSampleWebAppCommon.Queries
{
    public class GetShipsByNameQuery : IQuery
    {
        public string ShipName { get; set; }
    }
}