using Security.Application.Dtos.User;
using Security.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace Security.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == command.User.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Usuario con Id {command.User.Id} no encontrado");

            var person = await dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == user.PersonId, cancellationToken);

            if (person == null)
                throw new NotFoundException($"Persona con Id {user.PersonId} no encontrada");

            await ValidateBusinessRulesAsync(command, user, person, cancellationToken);

            // Actualizar Person
            UpdatePerson(person, command.User);

            // Actualizar User
            user.Update(command.User.Username, command.User.ProfileId);

            // Actualizar contraseña si se proporciona
            if (!string.IsNullOrWhiteSpace(command.User.Password))
            {
                var salt = User.GenerateSalt();
                var passwordHash = HashPassword(command.User.Password, salt);
                user.UpdateMyPassword(passwordHash, salt);
            }

            // Actualizar roles
            await UpdateUserRolesAsync(user, command.User.RolesId, cancellationToken);

            dbContext.Users.Update(user);
            dbContext.Persons.Update(person);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new UpdateUserResult(true);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException(e.Message, "Error interno al actualizar usuario");
        }
    }

    private async Task ValidateBusinessRulesAsync(UpdateUserCommand command, User existingUser, Person existingPerson, CancellationToken cancellationToken)
    {
        // Validar que el username no esté en uso por otro usuario
        if (existingUser.Username != command.User.Username)
        {
            await ValidateUsernameNotExistsAsync(command.User.Username, command.User.Id, cancellationToken);
        }

        // Validar que el documento no esté en uso por otra persona
        var normalizedDocNumber = command.User.DocumentNumber.Trim().ToUpper();
        if (existingPerson.DocumentType != command.User.DocumentType || 
            existingPerson.DocumentNumber != normalizedDocNumber)
        {
            await ValidateDocumentNumberNotExistsAsync(command.User.DocumentType, normalizedDocNumber, existingPerson.Id, cancellationToken);
        }
    }

    private async Task ValidateUsernameNotExistsAsync(string username, int currentUserId, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == username && x.Id != currentUserId, cancellationToken);

        if (existingUser != null)
            throw new BadRequestException($"El nombre de usuario {username} ya está en uso por otro usuario");
    }

    private async Task ValidateDocumentNumberNotExistsAsync(int documentType, string documentNumber, int currentPersonId, CancellationToken cancellationToken)
    {
        var existingPerson = await dbContext.Persons
            .FirstOrDefaultAsync(p => p.DocumentType == documentType && 
                                      p.DocumentNumber == documentNumber && 
                                      p.Id != currentPersonId, cancellationToken);

        if (existingPerson != null)
            throw new BadRequestException("El valor del campo número de documento ya está en uso por otra persona");
    }

    private void UpdatePerson(Person person, UserDto userDto)
    {
        person.DocumentType = userDto.DocumentType;
        person.DocumentNumber = userDto.DocumentNumber.Trim().ToUpper();
        person.FirstName = userDto.FirstName;
        person.LastName = userDto.LastName;
        person.Phone = userDto.Phone;
        person.Email = userDto.Email;
        person.Address = userDto.Address;
    }

    private async Task UpdateUserRolesAsync(User user, List<int> roleIds, CancellationToken cancellationToken)
    {
        // Validar que los roles existan
        var roles = await dbContext.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        var missingRoleIds = roleIds.Except(roles.Select(r => r.Id)).ToList();
        if (missingRoleIds.Count != 0)
        {
            throw new RolesNotFoundException(roleIds, "No se encontraron roles con los Ids proporcionados");
        }

        // Eliminar roles existentes del usuario
        var existingUserRoles = await dbContext.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync(cancellationToken);

        dbContext.UserRoles.RemoveRange(existingUserRoles);

        // Agregar nuevos roles directamente al contexto
        foreach (var roleId in roleIds)
        {
            var userRole = new UserRole 
            { 
                UserId = user.Id, 
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            };
            await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
        }
    }

    private string HashPassword(string password, string salt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(password));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("El salt no puede estar vacío.", nameof(salt));

        // Usar PBKDF2 (Rfc2898DeriveBytes) para generar el hash con el salt proporcionado
        const int iterations = 10000; // Número de iteraciones recomendado por OWASP
        const int hashLength = 32; // 256 bits

        var saltBytes = Convert.FromBase64String(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        using (var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
        {
            var hashBytes = pbkdf2.GetBytes(hashLength);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
