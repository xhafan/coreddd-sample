namespace CoreDddSampleConsoleApp.Domain
{
    public class TruckPolicyItem : CargoPolicyItem
    {
        protected TruckPolicyItem() { }

        public TruckPolicyItem(TruckPolicyItemArgs args) 
            : base(args.InsuredTonnage, args.RatePerTonnage)
        {
            Truck = args.Truck;
        }

        public virtual Truck Truck { get; protected set; }
    }
}