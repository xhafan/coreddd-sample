namespace CoreDddSampleConsoleApp.Domain
{
    public class TruckCargoPolicyItem : CargoPolicyItem
    {
        protected TruckCargoPolicyItem() { }

        public TruckCargoPolicyItem(TruckCargoPolicyItemArgs args) 
            : base(args.InsuredTonnage, args.RatePerTonnage)
        {
            Truck = args.Truck;
        }

        public virtual Truck Truck { get; protected set; }
    }
}