using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.MultipleQueries
{
    public class GetShipPolicyItemsByShipNameQuery : IQuery
    {
        public string ShipName { get; set; }

    }
}