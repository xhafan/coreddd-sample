﻿namespace CoreDddSampleConsoleApp.Domain
{
    public class TruckPolicyItemArgs
    {
        public Truck Truck { get; set; }
        public decimal InsuredTonnage { get; set; }
        public decimal RatePerTonnage { get; set; }
    }
}