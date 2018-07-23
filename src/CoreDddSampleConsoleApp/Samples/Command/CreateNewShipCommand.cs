using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Command
{
    public class CreateNewShipCommand : ICommand
    {
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}