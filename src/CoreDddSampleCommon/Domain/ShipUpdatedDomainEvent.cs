using CoreDdd.Domain.Events;

namespace CoreDddSampleWebAppCommon.Domain
{
    public class ShipUpdatedDomainEvent : IDomainEvent
    {
        public int ShipId { get; set; }
    }
}