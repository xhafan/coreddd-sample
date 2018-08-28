using CoreDdd.Domain.Events;

namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipCargoPolicyItemAddedDomainEvent : IDomainEvent
    {
        public int PolicyId { get; set; }
        public int ShipId { get; set; }
    }
}