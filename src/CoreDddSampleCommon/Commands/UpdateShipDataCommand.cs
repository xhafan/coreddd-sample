using CoreDdd.Commands;

namespace CoreDddSampleCommon.Commands
{
    public class UpdateShipDataCommand : ICommand
    {
        public int ShipId { get; set; }
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}