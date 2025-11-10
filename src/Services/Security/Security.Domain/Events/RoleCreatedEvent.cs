namespace Security.Domain.Events;
public record RoleCreatedEvent(Role Role) : IDomainEvent;
