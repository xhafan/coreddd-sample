using System;
using System.Collections.Generic;
using CoreDdd.Domain;

namespace CoreDddSampleConsoleApp.Domain
{
    public class Policy : Entity, IAggregateRoot
    {
        private readonly ICollection<PolicyItem> _items = new HashSet<PolicyItem>();

        protected Policy() {} // parameterless constructor needed by nhibernate to be able to instantiate the entity when loaded from database

        public Policy(
            PolicyHolder policyHolder,
            DateTime startDate,
            DateTime endDate,
            string terms
            )
        {
            PolicyHolder = policyHolder;
            StartDate = startDate;
            EndDate = endDate;
            Terms = terms;
        }

        public virtual PolicyHolder PolicyHolder { get; protected set; } // virtual modifier needed by nhibernate - https://stackoverflow.com/a/848116/379279
        public virtual DateTime StartDate { get; protected set; } // protected modifier needed by nhibernate - cannot be private
        public virtual DateTime EndDate { get; protected set; }
        public virtual string Terms { get; protected set; }

        public virtual IEnumerable<PolicyItem> Items => _items;

        public virtual void AddShipCargoPolicyItem(ShipCargoPolicyItemArgs args)
        {
            var shipCargoPolicyItem = new ShipCargoPolicyItem(args);
            _items.Add(shipCargoPolicyItem);
        }

        public virtual void AddTruckCargoPolicyItem(TruckCargoPolicyItemArgs args)
        {
            var truckCargoPolicyItem = new TruckCargoPolicyItem(args);
            _items.Add(truckCargoPolicyItem);
        }
    }
}