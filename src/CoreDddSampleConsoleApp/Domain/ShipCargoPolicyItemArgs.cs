namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipCargoPolicyItemArgs
    {
        public Ship Ship { get; set; }
        public decimal InsuredTonnage { get; set; }
        public decimal RatePerTonnage { get; set; }
    }
}