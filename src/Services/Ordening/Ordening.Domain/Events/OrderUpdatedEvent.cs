namespace Ordening.Domain.Events;
public record OrderUpdatedEvent(Order order) : IDomainEvent;