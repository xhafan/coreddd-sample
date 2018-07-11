using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp.Builders
{
    public class ShipBuilder
    {
        private string _name = "ship name";
        private decimal _tonnage = 10m;

        public ShipBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ShipBuilder WithTonnage(decimal tonnage)
        {
            _tonnage = tonnage;
            return this;
        }

        public Ship Build()
        {
            return new Ship(_name, _tonnage);
        }
    }
}