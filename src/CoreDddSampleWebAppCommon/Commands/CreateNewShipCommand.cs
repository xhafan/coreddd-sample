using CoreDdd.Commands;

namespace CoreDddSampleWebAppCommon.Commands
{
    public class CreateNewShipCommand : ICommand
    {
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}