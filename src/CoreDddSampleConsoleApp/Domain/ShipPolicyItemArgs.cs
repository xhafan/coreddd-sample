namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipPolicyItemArgs
    {
        public Ship Ship { get; set; }
        public decimal InsuredTonnage { get; set; }
        public decimal RatePerTonnage { get; set; }
    }
}