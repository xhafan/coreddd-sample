using CoreDdd.Domain;
using CoreDdd.Domain.Events;

namespace CoreDddSampleWebAppCommon.Domain
{
    public class Ship : Entity, IAggregateRoot
    {
        protected Ship() { } // parameterless constructor needed by nhibernate 
                             // to be able to instantiate the entity when loaded from database

        public Ship(string name, decimal tonnage)
        {
            Name = name;
            Tonnage = tonnage;
        }

        public virtual string Name { get; protected set; } // virtual modifier needed by nhibernate 
                                                           // - https://stackoverflow.com/a/848116/379279
        public virtual decimal Tonnage { get; protected set; } // protected modifier on set needed by nhibernate
                                                               // - cannot be private

        public virtual void UpdateData(string name, decimal tonnage)
        {
            Name = name;
            Tonnage = tonnage;

            DomainEvents.RaiseEvent(new ShipUpdatedDomainEvent { ShipId = Id });
        }
    }
}