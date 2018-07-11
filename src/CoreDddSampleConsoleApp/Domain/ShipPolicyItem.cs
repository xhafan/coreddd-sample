namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipPolicyItem : CargoPolicyItem
    {
        protected ShipPolicyItem() { }

        public ShipPolicyItem(ShipPolicyItemArgs args)
            : base(
                args.InsuredTonnage, 
                args.RatePerTonnage
                )
        {
            Ship = args.Ship;
        }

        public virtual Ship Ship { get; protected set; }
    }
}