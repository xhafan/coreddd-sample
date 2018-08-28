using CoreDdd.Domain.Events;

namespace CoreDddSampleConsoleApp.Domain
{
    public class ShipCargoPolicyItemAddedDomainEventHandler : IDomainEventHandler<ShipCargoPolicyItemAddedDomainEvent>
    {
        public void Handle(ShipCargoPolicyItemAddedDomainEvent domainEvent)
        {
            // handle the domain event here
        }
    }
}