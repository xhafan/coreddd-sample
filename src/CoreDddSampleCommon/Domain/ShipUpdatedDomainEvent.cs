using CoreDdd.Domain.Events;

namespace CoreDddSampleCommon.Domain
{
    public class ShipUpdatedDomainEvent : IDomainEvent
    {
        public int ShipId { get; set; }
    }
}