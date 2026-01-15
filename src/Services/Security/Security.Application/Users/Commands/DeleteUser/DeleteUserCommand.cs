using BuildingBlocks.CQRS;
using FluentValidation;

namespace Security.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(int UserId) : ICommand<DeleteUserResult>;
public record DeleteUserResult(bool Success);

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El Id del usuario es obligatorio")
            .GreaterThan(0)
            .WithMessage("El Id del usuario debe ser un número mayor a cero");
    }
}
