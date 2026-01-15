using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Security.Domain.Events;

namespace Security.Domain.Models;

public class User : Aggregate<int>
{
    public int PersonId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Salt { get; set; }
    //public string? CorporateEmail { get; set; }
    public int ProfileId { get; set; }
    public DateTime? LastAccessDate { get; set; }
    public bool IsActive { get; set; } = true;


    //Asignación de roles a usuarios
    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();


    public static User Create(int personId, string username, string passwordHash, string? salt, int profileId)
    {
        // Si no se proporciona salt, generar uno automáticamente
        var generatedSalt = salt ?? GenerateSalt();
        
        User user = new User
        {
            PersonId = personId,
            Username = username,
            PasswordHash = passwordHash,
            Salt = generatedSalt,
            ProfileId = profileId
        };
        user.AddDomainEvent(new UserCreatedEvent(user));

        return user;
    }

    public static string GenerateSalt(int saltLength = 32)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var saltBytes = new byte[saltLength];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }

    public void Update(string username, int profileId)
    {
        Username = username;
        ProfileId = profileId;
        AddDomainEvent(new UserUpdatedEvent(this)); 
    }
    
    public void ClearRoles()
    {
        _userRoles.Clear();
    }

    public void SetRoles(int roleId)
    {
        var userRole = new UserRole { UserId = Id, RoleId = roleId };
        _userRoles.Add(userRole);
    }

    public void UpdateMyPassword(string newPasswordHash, string? newSalt)
    {
        PasswordHash = newPasswordHash;
        Salt = newSalt;
        //LastModifiedBy = Id;
        AddDomainEvent(new UserUpdatedEvent(this));
    }

    public void Deactivate()
    {
        IsActive = false;
        AddDomainEvent(new UserDeletedEvent(this));
    }
}
