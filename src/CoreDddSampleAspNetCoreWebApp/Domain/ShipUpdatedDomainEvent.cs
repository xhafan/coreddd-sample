using CoreDdd.Domain.Events;

namespace CoreDddSampleAspNetCoreWebApp.Domain
{
    public class ShipUpdatedDomainEvent : IDomainEvent
    {
        public int ShipId { get; set; }
    }
}