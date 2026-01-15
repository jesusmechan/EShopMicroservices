namespace Auth.Domain.Events;
public record LogUpdatedEvent(AuthLog AuthLog) : IDomainEvent;