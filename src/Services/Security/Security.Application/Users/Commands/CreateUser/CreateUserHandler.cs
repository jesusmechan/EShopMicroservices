

using Microsoft.AspNetCore.Identity;
using Security.Application.Dtos.User;
using Security.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace Security.Application.Users.Commands.CreateUser;



public class CreateUserHandler (IApplicationDbContext dbContext) : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
		try
		{
            //Se busca si hay un usuario con el mismo username
            var existUserName = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == command.User.Username, cancellationToken);

            if(existUserName !=null)
                throw  new BadRequestException($"El nombre de usuario {command.User.Username} ya existe");

            var normalizedDocNumber = command.User.DocumentNumber.Trim().ToUpper();

            var existingNumberDocument = await dbContext.Persons
                    .FirstOrDefaultAsync(p => p.DocumentType == command.User.DocumentType && p.DocumentNumber == normalizedDocNumber, cancellationToken);

            if (existingNumberDocument != null)
            {
                throw new BadRequestException("El valor del campo número de documento ya está en uso");
            }

            // Se buscan los roles
            var roles = await dbContext.Roles
                .Where(r => command.User.RolesId.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var roleIds = command.User.RolesId;
            var missingRoleIds = roleIds.Except(roles.Select(r => r.Id)).ToList();
            if (missingRoleIds.Count != 0)
            {
                throw new RolesNotFoundException(roleIds, "No se encontraron roles con los Ids proporcionados");
            }
            var person = CreateNewPerson(command.User);

            await dbContext.Persons.AddAsync(person, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var user = CreateNewUser(command.User, person.Id, command.User.RolesId, "");

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new CreateUserResult(user.Id);
        }
		catch (Exception e)
		{

            var errorContent = $"Error al crear un usuario: {e.Message}";
            //var modelLog = new SecurityLog { Content = errorContent, CreatedAt = DateTime.UtcNow };
            //dbContext.SecurityLogs.Add(modelLog);

            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException(e.Message, "Error interno al crear usuario");
        }
    }

    private Person CreateNewPerson(UserDto user)
    {
        var newPerson = Person.Create(
            user.DocumentType,
            user.DocumentNumber.Trim().ToUpper(),
            user.FirstName,
            user.LastName,
            user.Phone,
            user.Email,
            user.Address,
            isActive: true
        );
        return newPerson;
    }


    private User CreateNewUser(UserDto user, int personId, List<int> roles, string generatedPassword)
    {
        var newUser = User.Create(
            personId,
            user.Username,
            generatedPassword,
            salt: null,
            user.ProfileId
        );
        foreach (var role in roles)
        {
            newUser.SetRoles(role);
        }

        return newUser;
    }


    private string GenerateRandomPassword(int length = 10)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var res = new StringBuilder();

        using (var rng = RandomNumberGenerator.Create())
        {
            var uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                var num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }

}
