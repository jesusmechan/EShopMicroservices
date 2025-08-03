
using BuildingBlocks.CQRS;
using MediatR;


namespace BuildingBlocks.CQRS;


//Cuando no devuelve nada
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{

}

//Cuando devuelve algo
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}