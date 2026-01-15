using Security.Domain.Models;

namespace Security.Domain.Events;

public record UserDeletedEvent(User User) : IDomainEvent;
