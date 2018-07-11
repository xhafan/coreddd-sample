namespace CoreDddSampleConsoleApp.Domain
{
    public abstract class CargoPolicyItem : PolicyItem
    {
        protected CargoPolicyItem() { }

        protected CargoPolicyItem(
            decimal insuredTonnage, 
            decimal ratePerTonnage
            )
        {
            InsuredTonnage = insuredTonnage;
            RatePerTonnage = ratePerTonnage;
        }

        public virtual decimal InsuredTonnage { get; protected set; }
        public virtual decimal RatePerTonnage { get; protected set; }
    }
}