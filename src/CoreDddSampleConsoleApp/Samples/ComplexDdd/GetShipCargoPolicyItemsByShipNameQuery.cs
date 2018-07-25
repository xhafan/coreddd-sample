using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class GetShipCargoPolicyItemsByShipNameQuery : IQuery
    {
        public string ShipName { get; set; }

    }
}