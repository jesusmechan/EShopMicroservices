

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
            await ValidateBusinessRulesAsync(command, cancellationToken);

            var (passwordHash, salt) = GeneratePasswordHash();

            var person = CreateNewPerson(command.User);
            await dbContext.Persons.AddAsync(person, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var roles = await GetValidatedRolesAsync(command.User.RolesId, cancellationToken);
            var user = CreateNewUser(command.User, person.Id, roles, passwordHash, salt);
            
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new CreateUserResult(user.Id);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException(e.Message, "Error interno al crear usuario");
        }
    }

    private async Task ValidateBusinessRulesAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await ValidateUsernameNotExistsAsync(command.User.Username, cancellationToken);
        await ValidateDocumentNumberNotExistsAsync(command.User.DocumentType, command.User.DocumentNumber, cancellationToken);
    }

    private async Task ValidateUsernameNotExistsAsync(string username, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        if (existingUser != null)
            throw new BadRequestException($"El nombre de usuario {username} ya existe");
    }

    private async Task ValidateDocumentNumberNotExistsAsync(int documentType, string documentNumber, CancellationToken cancellationToken)
    {
        var normalizedDocNumber = documentNumber.Trim().ToUpper();
        
        var existingPerson = await dbContext.Persons
            .FirstOrDefaultAsync(p => p.DocumentType == documentType && p.DocumentNumber == normalizedDocNumber, cancellationToken);

        if (existingPerson != null)
            throw new BadRequestException("El valor del campo número de documento ya está en uso");
    }

    private async Task<List<Role>> GetValidatedRolesAsync(List<int> roleIds, CancellationToken cancellationToken)
    {
        var roles = await dbContext.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        var missingRoleIds = roleIds.Except(roles.Select(r => r.Id)).ToList();
        if (missingRoleIds.Count != 0)
        {
            throw new RolesNotFoundException(roleIds, "No se encontraron roles con los Ids proporcionados");
        }

        return roles;
    }

    private (string passwordHash, string salt) GeneratePasswordHash()
    {
        var randomPassword = GenerateRandomPassword();
        var salt = User.GenerateSalt();
        var passwordHash = HashPassword(randomPassword, salt);
        
        return (passwordHash, salt);
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


    private User CreateNewUser(UserDto userDto, int personId, List<Role> roles, string passwordHash, string salt)
    {
        var newUser = User.Create(
            personId,
            userDto.Username,
            passwordHash,
            salt: salt,
            userDto.ProfileId
        );

        foreach (var role in roles)
        {
            newUser.SetRoles(role.Id);
        }

        return newUser;
    }


    private string GenerateRandomPassword(int length = 12)
    {
        const string lowers = "abcdefghijklmnopqrstuvwxyz";
        const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "1234567890";
        const string symbols = "!@#$%^&*";
        var all = lowers + uppers + digits + symbols;

        using var rng = RandomNumberGenerator.Create();
        var buffer = new byte[length];
        rng.GetBytes(buffer);

        var password = new StringBuilder();
        foreach (var b in buffer)
            password.Append(all[b % all.Length]);

        // Asegurar al menos un tipo de cada categoría
        if (!password.ToString().Any(char.IsLower)) password[0] = lowers[buffRand(lowers.Length)];
        if (!password.ToString().Any(char.IsUpper)) password[1] = uppers[buffRand(uppers.Length)];
        if (!password.ToString().Any(char.IsDigit)) password[2] = digits[buffRand(digits.Length)];
        if (!password.ToString().Any(c => symbols.Contains(c))) password[3] = symbols[buffRand(symbols.Length)];

        return password.ToString();

        int buffRand(int max) => buffer[RandomNumberGenerator.GetInt32(0, max)];
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
