using CoreDdd.Queries;

namespace CoreDddSampleAspNetCoreWebApp.Queries
{
    public class GetShipsByNameQuery : IQuery
    {
        public string ShipName { get; set; }
    }
}