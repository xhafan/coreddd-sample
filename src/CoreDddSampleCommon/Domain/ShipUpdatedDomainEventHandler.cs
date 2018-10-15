using CoreDdd.Domain.Events;

namespace CoreDddSampleWebAppCommon.Domain
{
    public class ShipUpdatedDomainEventHandler : IDomainEventHandler<ShipUpdatedDomainEvent>
    {
        public void Handle(ShipUpdatedDomainEvent domainEvent)
        {
            // publish an event over a message bus notifying that a ship data was updated
        }
    }
}