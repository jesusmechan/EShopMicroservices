using Security.Domain.Models;

namespace Security.Domain.Events;

public record PersonCreatedEvent(Person Person) : IDomainEvent;
