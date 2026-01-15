using Security.Domain.Events;

namespace Security.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteUserCommand, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Usuario con Id {command.UserId} no encontrado");

            if (!user.IsActive)
                throw new BadRequestException($"El usuario con Id {command.UserId} ya está desactivado");

            var person = await dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == user.PersonId, cancellationToken);

            if (person == null)
                throw new NotFoundException($"Persona con Id {user.PersonId} no encontrada");

            // Desactivar User (esto dispara el evento de dominio UserDeletedEvent)
            user.Deactivate();

            // Desactivar Person
            person.Deactivate();

            dbContext.Users.Update(user);
            dbContext.Persons.Update(person);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new DeleteUserResult(true);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException(e.Message, "Error interno al eliminar usuario");
        }
    }
}
