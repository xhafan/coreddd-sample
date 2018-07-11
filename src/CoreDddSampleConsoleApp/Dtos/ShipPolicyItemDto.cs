namespace CoreDddSampleConsoleApp.Dtos
{
    public class ShipPolicyItemDto
    {
        public int PolicyItemId { get; set; }
        public int PolicyId { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; }
    }
}