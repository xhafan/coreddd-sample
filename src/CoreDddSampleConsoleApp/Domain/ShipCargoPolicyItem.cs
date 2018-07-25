namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipCargoPolicyItem : CargoPolicyItem
    {
        protected ShipCargoPolicyItem() { }

        public ShipCargoPolicyItem(ShipCargoPolicyItemArgs args)
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