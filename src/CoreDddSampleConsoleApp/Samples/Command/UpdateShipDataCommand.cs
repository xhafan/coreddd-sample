using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class UpdateShipDataCommand : ICommand
    {
        public int ShipId { get; set; }
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}