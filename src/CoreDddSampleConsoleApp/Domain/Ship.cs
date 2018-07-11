using CoreDdd.Domain;

namespace CoreDddSampleConsoleApp.Domain
{
    public class Ship : Entity, IAggregateRoot
    {
        protected Ship() { }

        public Ship(string name, decimal tonnage)
        {
            Name = name;
            Tonnage = tonnage;
        }

        public virtual string Name { get; protected set; }
        public virtual decimal Tonnage { get; protected set; }
    }
}