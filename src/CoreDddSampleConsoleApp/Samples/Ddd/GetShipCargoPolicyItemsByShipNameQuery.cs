using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class GetShipCargoPolicyItemsByShipNameQuery : IQuery
    {
        public string ShipName { get; set; }

    }
}