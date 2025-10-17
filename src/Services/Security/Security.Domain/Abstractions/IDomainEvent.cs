using MediatR;

namespace Security.Domain.Abstractions;
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    DateTime OccurredOn => DateTime.Now; //Fecha de la ocurrencia del evento
    public string EventType => GetType().AssemblyQualifiedName;

}
