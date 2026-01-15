
namespace Auth.Domain.Events;

public record LogCreatedEvent(AuthLog AuthLog) : IDomainEvent;