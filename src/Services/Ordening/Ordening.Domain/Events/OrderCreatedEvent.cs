namespace Ordening.Domain.Events;
public record OrderCreatedEvent(Order order) : IDomainEvent;