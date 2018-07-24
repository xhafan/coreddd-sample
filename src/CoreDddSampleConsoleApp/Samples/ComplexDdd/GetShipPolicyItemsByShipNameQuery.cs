using CoreDdd.Queries;

namespace CoreDddSampleConsoleApp.Samples.ComplexDdd
{
    public class GetShipPolicyItemsByShipNameQuery : IQuery
    {
        public string ShipName { get; set; }

    }
}