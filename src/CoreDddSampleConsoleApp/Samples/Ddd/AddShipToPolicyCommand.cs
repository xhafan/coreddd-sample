using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class AddShipToPolicyCommand : ICommand
    {
        public int PolicyId { get; set; }
        public int ShipId { get; set; }
        public decimal InsuredTonnage { get; set; }
        public decimal RatePerTonnage { get; set; }
    }
}